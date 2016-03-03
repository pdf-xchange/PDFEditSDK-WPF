using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDFXEdit;


namespace WpfSimpleApp
{
	class SearchCbBridge : IPXV_SearchCallback
	{
		public void OnFinish(int nResCode)
		{
			;
		}

		public void OnNewEntry(IPXV_SearchEntry pEntry)
		{
			;
		}

		public void OnStart()
		{
			;
		}

		public void OnStartPtr(IPXV_SearchPtr pPtr)
		{
			string sNewProgText = "";
			for (uint i = 0; i < pPtr.Count; i++)
			{
				var ptrChunk = pPtr[i];
				//string str = pPtr.Str[i];
				switch ((PXV_SearchPtrChunkType)ptrChunk.nType)
				{
					case PXV_SearchPtrChunkType.PXV_SearchPtrChunk_Document:
						{
							var pDoc = pPtr.Doc[ptrChunk.nValue];
							if (sNewProgText != string.Empty)
								sNewProgText += " // ";
							sNewProgText += pDoc.SrcInfo.DispFileTitle;
							break;
						}
					case PXV_SearchPtrChunkType.PXV_SearchPtrChunk_Page:
						{
							//DFormattedString fmtStr(GetLocStr(loc_SearchInfoPageNo, str));
							//fmtStr.SetArg(1, (ptrChunk.nValue + 1));
							sNewProgText += ", ";
							sNewProgText += (ptrChunk.nValue + 1).ToString();
							break;
						}
					case PXV_SearchPtrChunkType.PXV_SearchPtrChunk_Bookmark:
						{
							//GetLocStr(loc_SearchInBookmarks, str);
							sNewProgText += ", ";
							sNewProgText += "SearchInBookmarks";
							break;
						}
					default:
						{
							if ((ptrChunk.nType & (int)PXV_SearchPtrChunkType.PXV_SearchPtrChunk_Object) != 0)
							{
								//GetLocStr(loc_SearchInObjects, str);
								sNewProgText += ", ";
								sNewProgText += "SearchInObjects";
							}
							break;
						}
				}
			}
		}

		public void OnStopPtr(IPXV_SearchPtr pPtr, bool bIncomplete)
		{
			//var str = pPtr.Str;
			;
		}
	}
}
