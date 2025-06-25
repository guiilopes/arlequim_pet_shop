using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[XmlRoot(ElementName = "ide")]
public class Ide
{
    [XmlElement(ElementName = "cUF")]
    public int CUF { get; set; }

    [XmlElement(ElementName = "cNF")]
    public int CNF { get; set; }

    [XmlElement(ElementName = "natOp")]
    public string NatOp { get; set; }

    [XmlElement(ElementName = "mod")]
    public int Mod { get; set; }

    [XmlElement(ElementName = "serie")]
    public int Serie { get; set; }

    [XmlElement(ElementName = "nNF")]
    public int NNF { get; set; }

    [XmlElement(ElementName = "dhEmi")]
    public DateTime DhEmi { get; set; }

    [XmlElement(ElementName = "dhSaiEnt")]
    public DateTime DhSaiEnt { get; set; }

    [XmlElement(ElementName = "tpNF")]
    public int TpNF { get; set; }

    [XmlElement(ElementName = "idDest")]
    public int IdDest { get; set; }

    [XmlElement(ElementName = "cMunFG")]
    public int CMunFG { get; set; }

    [XmlElement(ElementName = "tpImp")]
    public int TpImp { get; set; }

    [XmlElement(ElementName = "tpEmis")]
    public int TpEmis { get; set; }

    [XmlElement(ElementName = "cDV")]
    public int CDV { get; set; }

    [XmlElement(ElementName = "tpAmb")]
    public int TpAmb { get; set; }

    [XmlElement(ElementName = "finNFe")]
    public int FinNFe { get; set; }

    [XmlElement(ElementName = "indFinal")]
    public int IndFinal { get; set; }

    [XmlElement(ElementName = "indPres")]
    public int IndPres { get; set; }

    [XmlElement(ElementName = "indIntermed")]
    public int IndIntermed { get; set; }

    [XmlElement(ElementName = "procEmi")]
    public int ProcEmi { get; set; }

    [XmlElement(ElementName = "verProc")]
    public string VerProc { get; set; }
}

[XmlRoot(ElementName = "enderEmit")]
public class EnderEmit
{
    [XmlElement(ElementName = "xLgr")]
    public string XLgr { get; set; }

    [XmlElement(ElementName = "nro")]
    public string Nro { get; set; }

    [XmlElement(ElementName = "xBairro")]
    public string XBairro { get; set; }

    [XmlElement(ElementName = "cMun")]
    public int CMun { get; set; }

    [XmlElement(ElementName = "xMun")]
    public string XMun { get; set; }

    [XmlElement(ElementName = "UF")]
    public string UF { get; set; }

    [XmlElement(ElementName = "CEP")]
    public int CEP { get; set; }

    [XmlElement(ElementName = "cPais")]
    public int CPais { get; set; }

    [XmlElement(ElementName = "xPais")]
    public string XPais { get; set; }

    [XmlElement(ElementName = "fone")]
    public string Fone { get; set; }
}

[XmlRoot(ElementName = "emit")]
public class Emit
{
    [XmlElement(ElementName = "CNPJ")]
    public string CNPJ { get; set; }

    [XmlElement(ElementName = "xNome")]
    public string XNome { get; set; }

    [XmlElement(ElementName = "xFant")]
    public string XFant { get; set; }

    [XmlElement(ElementName = "enderEmit")]
    public EnderEmit EnderEmit { get; set; }

    [XmlElement(ElementName = "IE")]
    public string IE { get; set; }

    [XmlElement(ElementName = "IM")]
    public string IM { get; set; }

    [XmlElement(ElementName = "CNAE")]
    public string CNAE { get; set; }

    [XmlElement(ElementName = "CRT")]
    public int CRT { get; set; }
}

[XmlRoot(ElementName = "enderDest")]
public class EnderDest
{
    [XmlElement(ElementName = "xLgr")]
    public string XLgr { get; set; }

    [XmlElement(ElementName = "nro")]
    public string Nro { get; set; }

    [XmlElement(ElementName = "xBairro")]
    public string XBairro { get; set; }

    [XmlElement(ElementName = "cMun")]
    public int CMun { get; set; }

    [XmlElement(ElementName = "xMun")]
    public string XMun { get; set; }

    [XmlElement(ElementName = "UF")]
    public string UF { get; set; }

    [XmlElement(ElementName = "CEP")]
    public int CEP { get; set; }

    [XmlElement(ElementName = "cPais")]
    public int CPais { get; set; }

    [XmlElement(ElementName = "xPais")]
    public string XPais { get; set; }

    [XmlElement(ElementName = "fone")]
    public string Fone { get; set; }
}

[XmlRoot(ElementName = "dest")]
public class Dest
{
    [XmlElement(ElementName = "CNPJ")]
    public string CNPJ { get; set; }

    [XmlElement(ElementName = "xNome")]
    public string XNome { get; set; }

    [XmlElement(ElementName = "enderDest")]
    public EnderDest EnderDest { get; set; }

    [XmlElement(ElementName = "indIEDest")]
    public int IndIEDest { get; set; }

    [XmlElement(ElementName = "IE")]
    public string IE { get; set; }
}

[XmlRoot(ElementName = "prod")]
public class Prod
{
    [XmlElement(ElementName = "cProd")]
    public string CProd { get; set; }

    [XmlElement(ElementName = "cEAN")]
    public string BarCode { get; set; }

    [XmlElement(ElementName = "xProd")]
    public string Name { get; set; }

    [XmlElement(ElementName = "NCM")]
    public string NCM { get; set; }

    [XmlElement(ElementName = "CFOP")]
    public string CFOP { get; set; }

    [XmlElement(ElementName = "uCom")]
    public string UCom { get; set; }

    [XmlElement(ElementName = "qCom")]
    public decimal QCom { get; set; }

    [XmlElement(ElementName = "vUnCom")]
    public decimal Price { get; set; }

    [XmlElement(ElementName = "vProd")]
    public decimal VProd { get; set; }

    [XmlElement(ElementName = "cEANTrib")]
    public string CEANTrib { get; set; }

    [XmlElement(ElementName = "uTrib")]
    public string UTrib { get; set; }

    [XmlElement(ElementName = "qTrib")]
    public decimal QTrib { get; set; }

    [XmlElement(ElementName = "vUnTrib")]
    public decimal VUnTrib { get; set; }

    [XmlElement(ElementName = "indTot")]
    public int IndTot { get; set; }
}

[XmlRoot(ElementName = "det")]
public class Det
{
    [XmlElement(ElementName = "prod")]
    public Prod Prod { get; set; }

    [XmlAttribute(AttributeName = "nItem")]
    public int NItem { get; set; }
}

[XmlRoot(ElementName = "infNFe")]
public class InfNFe
{
    [XmlElement(ElementName = "ide")]
    public Ide Ide { get; set; }

    [XmlElement(ElementName = "emit")]
    public Emit Emit { get; set; }

    [XmlElement(ElementName = "dest")]
    public Dest Dest { get; set; }

    [XmlElement(ElementName = "det")]
    public List<Det> Det { get; set; }

    [XmlAttribute(AttributeName = "Id")]
    public string Id { get; set; }

    [XmlAttribute(AttributeName = "versao")]
    public string Versao { get; set; }
}

[XmlRoot(ElementName = "NFe")]
public class NFe
{
    [XmlElement(ElementName = "infNFe")]
    public InfNFe InfNFe { get; set; }

    [XmlAttribute(AttributeName = "xmlns")]
    public string Xmlns { get; set; }
}

[XmlRoot(ElementName = "protNFe")]
public class ProtNFe
{
    [XmlElement(ElementName = "infProt")]
    public InfProt InfProt { get; set; }

    [XmlAttribute(AttributeName = "versao")]
    public string Versao { get; set; }
}

[XmlRoot(ElementName = "infProt")]
public class InfProt
{
    [XmlElement(ElementName = "tpAmb")]
    public int TpAmb { get; set; }

    [XmlElement(ElementName = "verAplic")]
    public string VerAplic { get; set; }

    [XmlElement(ElementName = "chNFe")]
    public string ChNFe { get; set; }

    [XmlElement(ElementName = "dhRecbto")]
    public DateTime DhRecbto { get; set; }

    [XmlElement(ElementName = "nProt")]
    public string NProt { get; set; }

    [XmlElement(ElementName = "digVal")]
    public string DigVal { get; set; }

    [XmlElement(ElementName = "cStat")]
    public int CStat { get; set; }

    [XmlElement(ElementName = "xMotivo")]
    public string XMotivo { get; set; }
}

[XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
public class ProductDocumentFiscalDto
{
    [XmlElement(ElementName = "NFe")]
    public NFe NFe { get; set; }

    [XmlElement(ElementName = "protNFe")]
    public ProtNFe ProtNFe { get; set; }

    [XmlAttribute(AttributeName = "versao")]
    public string Versao { get; set; }

    [XmlAttribute(AttributeName = "xmlns")]
    public string Xmlns { get; set; }
}