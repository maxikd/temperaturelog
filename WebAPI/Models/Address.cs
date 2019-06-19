using Newtonsoft.Json;

namespace WebAPI.Models
{
    public class Address
    {
        //{
        //  "cep": "25675-012",
        //  "logradouro": "Rua Mosela",
        //  "complemento": "de 1166 a 1734 - lado par",
        //  "bairro": "Mosela",
        //  "localidade": "Petrópolis",
        //  "uf": "RJ",
        //  "unidade": "",
        //  "ibge": "3303906",
        //  "gia": ""
        //}

        [JsonProperty("cep")]
        public string PostalCode { get; set; }

        [JsonProperty("logradouro")]
        public string Street { get; set; }

        [JsonProperty("complemento")]
        public string ExtraInfo { get; set; }

        [JsonProperty("bairro")]
        public string Neighborhood { get; set; }

        [JsonProperty("localidade")]
        public string City { get; set; }

        [JsonProperty("uf")]
        public string State { get; set; }

        [JsonProperty("unidade")]
        public string Unity { get; set; }

        [JsonProperty("ibge")]
        public string IBGE { get; set; }

        [JsonProperty("gia")]
        public string Gia { get; set; }
    }
}
