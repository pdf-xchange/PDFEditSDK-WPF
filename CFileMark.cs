using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFXEdit;
using System.Runtime.InteropServices;
using System.Windows;

namespace WpfSimpleApp
{
	class CFileMark
	{
		private IPXV_Inst Inst = null;
		private IPXC_Document Doc = null;

		public CFileMark(IPXV_Inst inst, IPXC_Document doc)
		{
			this.Inst = inst;
			this.Doc = doc;
		}

		public void SaveFile(string fileName)
		{
			try
			{
				Doc.WriteToFile(fileName, null, 0);
				System.Diagnostics.Process openProcess = new System.Diagnostics.Process();
				openProcess.StartInfo.FileName = fileName;
				openProcess.StartInfo.UseShellExecute = true;
				openProcess.StartInfo.CreateNoWindow = true;
				openProcess.StartInfo.Verb = "open";
				openProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
				openProcess.Start();
				openProcess.WaitForExit();
			}
			catch (COMException e)
			{
				MessageBox.Show(string.Format("Document has been saved to '{0}'. {1}", fileName, e.Message));
			};
		}

		public void Run()
		{
			
			int nID = Inst.Str2ID("op.search", false);
			PDFXEdit.IOperation pOp = Inst.CreateOp(nID);
			PDFXEdit.ICabNode input = pOp.Params.Root["Input"];
			input.Add().v = Doc;
			PDFXEdit.ICabNode options = pOp.Params.Root["Options"];
			options["StartPage"].v = 0;
			options["StopPage"].v = Doc.Pages.Count;
			int nFlags = (int)PXV_SearchFlags.PXV_SearchFlag_IncludeBookmarks;
			options["Flags"].v = nFlags;
			PDFXEdit.ICabNode arrAnd = options["AND"];
			arrAnd.Add().v = "the";
			options["Callback"].v = new SearchCbBridge();
			pOp.Do();
		}

	}
}
