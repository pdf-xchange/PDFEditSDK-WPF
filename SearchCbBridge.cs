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
			;
		}

		public void OnStopPtr(IPXV_SearchPtr pPtr, bool bIncomplete)
		{
			;
		}
	}
}
