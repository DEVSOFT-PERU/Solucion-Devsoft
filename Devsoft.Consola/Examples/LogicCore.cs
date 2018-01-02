using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devsoft.Consola.Examples
{
    public class LogicCore
    {
        public LogicCore()
        {
            Core.Util.Constantes.CNX_STRING = "Data Source=MARKANTHONYARRO;Initial Catalog=SBO_LOCA_V1;Persist Security Info=True;User ID=sa;Password=sql;";
        }

        public  void ejecutar()
        {
            Core.Helper.SqlDatabaseHelper.Execute("SELECT * FROM OITM", type: CommandType.Text);
            Core.Helper.SqlDatabaseHelper.Execute("TEST_SELECTS");
        }
    }
}
