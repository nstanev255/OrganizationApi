using System.Text;
using Microsoft.Extensions.Primitives;
using OrganizationApi.Entity;

namespace OrganizationApi.Services.Impl;

public class FileServiceImpl : IFileService
{
    public PdfDocument CreatePdfForOrganization(Organization organization)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append($"<h1> <a href = \"{organization.Website}\"> Name : {organization.Name} | Index: {organization.Id} | Id : {organization.OrganizationId} </a> </h1>");
        sb.Append($"<h2> Description: {organization.Description} </h2>");
        sb.Append("</br>");
        sb.Append($"<h2> Founded {organization.Founded}</h2>");
        sb.Append("</br>");
        sb.Append($"<h2>Employees: {organization.NumberOfEmployees}</h2>");

        sb.Append("</hr>");
        
        if (organization.Industry != null)
        {
            sb.Append("<h2> Industry </h2>");
            sb.Append($"<h3> Id : {organization.Industry.Id} </h3>");
            sb.Append($"<h3> Name : {organization.Industry.Name}");
            sb.Append("</hr>");
        }

        if (organization.Country != null)
        {
            sb.Append("<h2> Country </h2>");
            sb.Append($"<h3> Id : {organization.Country.Id} </h3>");
            sb.Append($"<h3> Name : {organization.Country.Name} </h3>");
            if (organization.Country.Code != null)
            {
                sb.Append($"<h3> Country Code : {organization.Country.Code} </h3>");
            }

            sb.Append("</hr>");
        }

        return CreatePdfFromHtml(sb.ToString());
    }

    public PdfDocument CreatePdfFromHtml(string html)
    {
        var renderer = new ChromePdfRenderer();
        return renderer.RenderHtmlAsPdf(html);
    }
}