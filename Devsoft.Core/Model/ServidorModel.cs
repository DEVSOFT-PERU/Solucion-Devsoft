namespace Devsoft.Core.Model
{
    public class ServidorModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string IP { get; set; }
        public string Puerto { get; set; }
        public string TipoConexion { get; set; }
        public string TipoBD { get; set; }
        public string ServidorLicencia { get; set; }
        public string PuertoLicencia { get; set; }
        public string UsuarioServ { get; set; }
        public string ContraServ { get; set; }
    }
}
