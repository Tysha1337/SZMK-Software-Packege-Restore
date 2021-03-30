using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SZMK.Desktop.Models;
using SZMK.Desktop.Views.Shared;

namespace SZMK.Desktop.Services
{
    public class PDFService
    {
        public bool CombineDetails(List<Order> selected, ForLongOperations_F dialog)
        {
            try
            {
                PdfDocument outputDocument = new PdfDocument();
                List<Specific> specifics = new List<Specific>();

                dialog.SetMaximum(selected.Count);

                for (int i = 0; i < selected.Count; i++)
                {
                    List<Detail> details = SystemArgs.Request.GetDetails(selected[i].ID);
                    string pathDetails = selected[i].PathDetails.PathPDF;

                    for (int j = 0; j < details.Count; j++)
                    {
                        if (specifics.FindAll(p => p.Number == selected[i].Number && p.NumberSpecific == details[j].Position).Count == 0)
                        {
                            if (!String.IsNullOrEmpty(details[j].Name))
                            {
                                if (File.Exists(pathDetails + @"\" + details[j].Name + ".pdf"))
                                {
                                    PdfDocument inputDocument = PdfReader.Open(pathDetails + @"\" + details[j].Name + ".pdf", PdfDocumentOpenMode.Import);

                                    foreach (PdfPage page in inputDocument.Pages)
                                    {
                                        outputDocument.AddPage(page);
                                    }

                                    specifics.Add(new Specific(selected[i].Number, details[j].Position, pathDetails + @"\" + details[j].Name + ".pdf", details[j].Count, true));
                                }
                                else
                                {
                                    specifics.Add(new Specific(selected[i].Number, details[j].Position, pathDetails + @"\" + details[j].Name + ".pdf", details[j].Count, false));
                                }
                            }
                            else
                            {
                                if (File.Exists(pathDetails + @"\" + "Дет." + details[j].Position + ".pdf"))
                                {
                                    PdfDocument inputDocument = PdfReader.Open(pathDetails + @"\" + "Дет." + details[j].Position + ".pdf", PdfDocumentOpenMode.Import);

                                    foreach (PdfPage page in inputDocument.Pages)
                                    {
                                        outputDocument.AddPage(page);
                                    }

                                    specifics.Add(new Specific(selected[i].Number, details[j].Position, pathDetails + @"\" + "Дет." + details[j].Position + ".pdf", details[j].Count, true));
                                }
                                else
                                {
                                    specifics.Add(new Specific(selected[i].Number, details[j].Position, pathDetails + @"\" + "Дет." + details[j].Position + ".pdf", details[j].Count, false));
                                }
                            }
                        }
                        else
                        {
                            specifics.First(p => p.Number == selected[i].Number && p.NumberSpecific == details[j].Position).Count += details[j].Count;
                        }
                    }

                    dialog.Notify(i + 1, $"Объединение чертежа {i + 1} из {selected.Count}");
                }

                Directory.CreateDirectory(@"TempPrint");

                if (outputDocument.PageCount > 0)
                {
                    outputDocument.Save(@"TempPrint\combineselecteddetails.pdf");
                }

                dialog.Invoke((MethodInvoker)delegate ()
                {
                    dialog.Close();

                    ReportPrintSpecific reportPrint = new ReportPrintSpecific(specifics);

                    reportPrint.ShowDialog();
                });

                Directory.Delete(@"TempPrint", true);

                return true;
            }
            catch (Exception E)
            {
                SystemArgs.PrintLog(E.ToString());
                return false;
            }
        }
    }
}
