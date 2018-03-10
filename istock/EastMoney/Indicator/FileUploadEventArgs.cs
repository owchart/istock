using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class FileUploadEventArgs : EventArgs
    {
        private String _fileUri;
        private String _id;

        public FileUploadEventArgs(String id, String fileUri)
        {
            this._id = id;
            this._fileUri = fileUri;
        }

        public String FileUri
        {
            get
            {
                return this._fileUri;
            }
        }

        public String Id
        {
            get
            {
                return this._id;
            }
        }
    }
}
