using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BART.SP.OCR.CP.Model
{
    public interface IStandardAction
    {
        bool New();
        bool Update();
        bool Delete();
    }
}
