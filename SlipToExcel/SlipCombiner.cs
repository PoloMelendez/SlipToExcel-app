using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlipToExcel
{
    //Covers the use case when a packing slip is sent in as
    //multiple text files
    class SlipCombiner
    {
        public List<SlipBox> fileList;
        public string outFile;

        public SlipCombiner(List<SlipBox> list, string outName)
        {
            fileList = list;
            outFile = outName;
        }

        public void combine()
        {
            using (var output = File.Create(outFile))
            {
                foreach (var file in fileList)
                {
                    using (var input = File.OpenRead(file.slipPath))
                    {
                        input.CopyTo(output);
                    }
                }
            }
        }
    }
}
