﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using PDFXEdit;

namespace WpfSimpleApp
{
	class CSampleFill
	{
		public string mySDKKey = "";
		public PXC_Rect letterSize = CreateRect(0.0, 0.0, I2P(8.5), I2P(11.0));

		public Color clrBlack = Color.FromRgb(0, 0, 0);
		public Color clrWhite = Color.FromRgb(255, 255, 255);
		public Color clrKhaki = Color.FromRgb(107, 183, 189);
		public uint clrLtGray = RGB(232, 232, 232);

		public PXV_Inst g_VInst = new PXV_Inst();
		public PXC_Inst g_Inst = null;

		static uint RGB(uint r, uint g, uint b)
		{
			return (r + (g << 8) + (b << 16));
		}

		static double I2P(double x)
		{
			return (x * 72.0);
		}
		static void SetRect(ref PXC_Rect rc, double left, double bottom, double right, double top)
		{
			rc.bottom = bottom;
			rc.left = left;
			rc.right = right;
			rc.top = top;
		}
		static PXC_Rect CreateRect(double left, double bottom, double right, double top)
		{
			PXC_Rect rc = new PXC_Rect();
			SetRect(ref rc, left, bottom, right, top);
			return rc;
		}
		public static uint LoWord(uint dwValue)
		{
			return (dwValue & 0xFFFF);
		}
		public static uint HiWord(uint dwValue)
		{
			return (dwValue >> 16) & 0xFFFF;
		}
		public static uint LoByte(uint dwValue)
		{
			return (dwValue & 0xFF);
		}
		public static uint HiByte(uint dwValue)
		{
			return (dwValue >> 8) & 0xFF;
		}
		public void InitializeSDK()
		{
			g_VInst.Init(null, mySDKKey);
			g_Inst = g_VInst.GetExtension("PXC");
			//          IPXS_Inst g_COS = g_VInst.GetExtension("PXS");
			//          IIXC_Inst g_ImgCore = g_VInst.GetExtension("IXC");
			//          IAUX_Inst g_AUX = g_VInst.GetExtension("AUX");
		}

		public void FinalizeSDK()
		{
			g_Inst = null;
			g_VInst.Shutdown();
			g_VInst = null;
		}

		private void CreateNewDocWithPage(out IPXC_Document pDoc, out IPXC_Page pPage, PXC_Rect pageRect)
		{
			// new document creation
			pDoc = g_Inst.NewDocument();
			pDoc.Props.SpecVersion = 0x10007;
			AddNewPage(ref pDoc, out pPage, pageRect);
		}
		private void AddNewPage(ref IPXC_Document pDoc, out IPXC_Page pPage, PXC_Rect pageRect)
		{
			pPage = null;
			//
			IPXC_Pages pPages = pDoc.Pages;
			// adding page to the document
			PXC_Rect pr = pageRect;
			IPXC_UndoRedoData pUndoData;
			pPage = pPages.InsertPage(uint.MaxValue, ref pr, out pUndoData);
			pUndoData = null;
			pPages = null;
		}

		private void DrawTitle(ref IPXC_Document pDoc, IPXC_ContentCreator pCC, double cx, double baseLineY, string sText, double fontSize)
		{
			IPXC_Font pFont = pDoc.CreateNewFont("Arial", 0, 400);
			pCC.SaveState();
			pCC.SetFillColorRGB(RGB(clrBlack.R, clrBlack.G, clrBlack.B));
			pCC.SetFont(pFont);
			double twidth = 0;
			double theight = 0;
			pCC.CalcTextSize(fontSize, sText, out twidth, out theight, -1);
			pCC.SetFontSize(fontSize);
			pCC.ShowTextLine(cx - twidth / 2.0, baseLineY, sText, -1, (uint)PXC_ShowTextLineFlags.STLF_Top | (uint)PXC_ShowTextLineFlags.STLF_AllowSubstitution);
			pCC.RestoreState();
			pFont = null;
		}
		private void Fill_AddStarPath(IPXC_ContentCreator pCC, double x, double y, double r)
		{
			const int num = 5;
			double[] points = new double[num * 2];

			double a = -90;
			for (int i = 0; i < num; i++)
			{
				points[i * 2 + 0] = x + r * Math.Cos(a * Math.PI / 180.0);
				points[i * 2 + 1] = y - r * Math.Sin(a * Math.PI / 180.0);
				a += 2.0 * (360.0 / num);
			}
			pCC.PolygonSA(points, true);
		}

		private void Fill_Ex1(ref IPXC_Document pDoc, IPXC_ContentCreator pCC, PXC_Rect pr)
		{
			double x = I2P(2.0);
			double y = pr.top - I2P(2.0);
			double r = I2P(1.0);
			double rr;

			string[] titles = { "NONZERO WINDING NUMBER RULE", "EVEN-ODD RULE" };
			PXC_FillRule[] rules = { PXC_FillRule.FillRule_Winding, PXC_FillRule.FillRule_EvenOdd };

			for (int i = 0; i< titles.Length; i++)
			{
				x = I2P(2.0);
				PXC_FillRule rule = rules[i];
				DrawTitle(ref pDoc, pCC, (pr.right + pr.left) / 2, y - r - 15, titles[i], 15);
				//
				Fill_AddStarPath(pCC, x, y, r);
				pCC.SetStrokeColorRGB(RGB(clrBlack.R, clrBlack.G, clrBlack.B));
				pCC.SetFillColorRGB(RGB(clrKhaki.R, clrKhaki.G, clrKhaki.B));
				pCC.FillPath(true, true, rule);

				x = (pr.right + pr.left) / 2;
				rr = r;
				pCC.Arc(CreateRect( x - rr, y - rr, x + rr, y + rr), 0.0, 360.0, true);
				rr = r / 2;
				pCC.Arc(CreateRect( x - rr, y - rr, x + rr, y + rr), 0.0, 360.0, true);
				pCC.FillPath(true,true, rule);

				x = pr.right - I2P(2);
				rr = r;
				pCC.Arc(CreateRect( x - rr, y - rr, x + rr, y + rr), 0.0, 360.0, true);
				rr = r / 2;
				pCC.Arc(CreateRect( x - rr, y - rr, x + rr, y + rr), 360.0, 0.0, true);
				pCC.FillPath(true, true, rule);
				//
				y -= I2P(3);
			}
		}

		private void Fill_Ex2(ref IPXC_Document pDoc, IPXC_ContentCreator pCC, PXC_Rect pr)
		{
			double w = (pr.right - pr.left - I2P(3)) / 2.0;
			double h = I2P(1);
			double y = pr.top - I2P(1.0) - h;
			double dy = I2P(2);
			double[] x = new double[2] { pr.left + I2P(1.0), pr.left + I2P(1.0 + 4.0) };

			pCC.SetLineWidth(1.0);
			pCC.SetStrokeColorRGB(RGB(clrBlack.R, clrBlack.G, clrBlack.B));
			pCC.SetFillColorRGB(clrLtGray);

			pCC.Rect(x[0], y, x[0] + w, y + h);
			pCC.StrokePath(false);
			DrawTitle(ref pDoc, pCC, x[0] + w / 2, y - I2P(0.1), "STROKE, NO FILL", 15);

			pCC.Rect(x[1], y, x[1] + w, y + h);
			pCC.FillPath(false, false, PXC_FillRule.FillRule_Winding);
			DrawTitle(ref pDoc, pCC, x[1] + w / 2, y - I2P(0.1), "FILL, NO STROKE", 15);

			y -= dy;

			pCC.Rect(x[0], y, x[0] + w, y + h);
			pCC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
			DrawTitle(ref pDoc, pCC, x[0] + w / 2, y - I2P(0.1), "STROKE & FILL", 15);

			string[] titles =
			{
				"PATTER FILL: CrossHatch",
				"PATTER FILL: CrossDiagonal",
				"PATTER FILL: DiagonalLeft",
				"PATTER FILL: DiagonalRight",
				"PATTER FILL: Horizontal",
				"PATTER FILL: Vertical"
			};

			int k = 1;
			IPXC_Pattern pPat;
			for (int i = (int)PXC_StdPatternType.StdPattern_CrossHatch; i <= (int)PXC_StdPatternType.StdPattern_Vertical; i++)
			{

				pPat = pDoc.GetStdTilePattern((PXC_StdPatternType)i);
				pCC.SetPatternRGB(pPat, true, RGB(clrKhaki.R, clrKhaki.G, clrKhaki.B));
				pPat = null;
				pCC.Rect(x[k], y, x[k] + w, y + h);
				pCC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
				DrawTitle(ref pDoc, pCC, x[k] + w / 2, y - I2P(0.1), titles[i - (int)PXC_StdPatternType.StdPattern_CrossHatch], 15);
				k ^= 1;
				if (k == 0)
					y -= dy;
			}

//             CBitmap bmp;
//             bmp.LoadBitmap(IDB_MAIN);
//             hr = CreateImagePattern(pDoc, bmp, pPat);

			//pCC.SetPatternRGB(pPat, true, (uint)clrKhaki.ToArgb());
			pCC.Rect(x[k], y, x[k] + w, y + h);
			pCC.FillPath(false, true, PXC_FillRule.FillRule_Winding);
			DrawTitle(ref pDoc, pCC, x[k] + w / 2, y - I2P(0.1), "PATTERN FILL: Image", 15);
		}

		public void Perform(string fileName)
		{
			try
			{
				// creating document with one page
				IPXC_Document pDoc;
				IPXC_Page pPage;

				PXC_Rect pr = letterSize;
				CreateNewDocWithPage(out pDoc, out pPage, pr);

				// creating some conent
				IPXC_ContentCreator pCC = pDoc.CreateContentCreator();
				// page 1
				Fill_Ex1(ref pDoc, pCC, pr);
				IPXC_Content pContent = pCC.Detach();
				pPage.PlaceContent(pContent, (UInt32)PXC_PlaceContentFlags.PlaceContent_Replace);
				pContent = null;
				

				// page 2
				AddNewPage(ref pDoc, out pPage, pr);
				Fill_Ex2(ref pDoc, pCC, pr);
				pContent = pCC.Detach();
				pPage.PlaceContent(pContent, (UInt32)PXC_PlaceContentFlags.PlaceContent_Replace);
				pContent = null;
				pCC = null;
				pPage = null;


				// Adding file attachment
				IPXC_NameTree pTree = pDoc.GetNameTree("EmbeddedFiles");
				string filePath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\TestFile.pdf";
				IPXC_FileSpec pFS = pDoc.CreateEmbeddFile("Test.pdf");
				IPXC_EmbeddedFileStream pEFS = pFS.EmbeddedFile;
				pEFS.UpdateFromFile2(filePath);
				pTree.Add("Test", pFS.PDFObject);

				SaveDocument(ref pDoc, fileName);
				if (pDoc != null)
					pDoc.Close();
				pDoc = null;
			}
			catch (COMException e)
			{
				MessageBox.Show(e.Message);
			};
		}

		private void SaveDocument(ref IPXC_Document pDoc, string fileName)
		{
			try
			{
				pDoc.WriteToFile(fileName, null, 0);
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
				MessageBox.Show(string.Format("Document has been saved to '{0}'. {1}", fileName,e.Message));
			};
		}

		public CFileMark OpenFile(string fileName)
		{
			var doc = g_Inst.OpenDocumentFromFile(fileName, null);
			return new CFileMark(g_VInst, doc);
		}
	}


}
