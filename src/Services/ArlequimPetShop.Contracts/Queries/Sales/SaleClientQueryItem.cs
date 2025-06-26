namespace ArlequimPetShop.Contracts.Queries.Sales
{
    /// <summary>
    /// Representa os dados do cliente associados a uma venda.
    /// </summary>
    public class SaleClientQueryItem
    {
        /// <summary>
        /// Identificador do cliente.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do cliente.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Documento do cliente (CPF/CNPJ).
        /// </summary>
        public string? Document { get; set; }
    }
}