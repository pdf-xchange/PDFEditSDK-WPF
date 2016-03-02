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
			throw new NotImplementedException();
		}

		public void OnNewEntry(IPXV_SearchEntry pEntry)
		{
			throw new NotImplementedException();
		}

		public void OnStart()
		{
			throw new NotImplementedException();
		}

		public void OnStartPtr(IPXV_SearchPtr pPtr)
		{
			throw new NotImplementedException();
		}

		public void OnStopPtr(IPXV_SearchPtr pPtr, bool bIncomplete)
		{
			throw new NotImplementedException();
		}
	}
}
