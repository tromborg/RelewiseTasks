using Relewise.Client.DataTypes;
using System;
using System.Collections;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

[XmlRoot("rss")]
public class RssFeed
{
    [XmlElement("channel")]
    public Channel Channel { get; set; }
}

public class Channel
{
    [XmlElement("item")]
    public List<Item> Products { get; set; }
}


public class Item
{
    [XmlElement(ElementName = "id")]
    public string Id { get; set; }

    [XmlElement("title")]
    public string Title { get; set; }

    [XmlElement(ElementName = "price")]
    public decimal Price { get; set; }

    [XmlElement(ElementName = "sale_price")]
    public decimal SalePrice { get; set; }
}



public class JobServiceJSON: IJob{
    public async Task<string> Execute(JobArguments arguments, Func<string, Task> info, Func<string, Task> warn, CancellationToken token){
        string url = "https://cdn.relewise.com/academy/productdata/customjsonfeed";

        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync(url);
        var products = JsonSerializer.Deserialize<List<Relewise.Client.DataTypes.Product>>(response);

        var count = products == null ? 0 : products?.Count;
        string countMessage = "Execute ran and the total count was: " + count + "(JSON)";
        Console.WriteLine(countMessage);
        return countMessage;

    }
}

public class JobServiceXML: IJob{
    public async Task<string> Execute(JobArguments arguments, Func<string, Task> info, Func<string, Task> warn, CancellationToken token){
        string url = "https://cdn.relewise.com/academy/productdata/googleshoppingfeed";

        HttpClient client = new HttpClient();
        var response = await client.GetStringAsync(url);
        var products = new XmlSerializer(typeof(RssFeed));
        RssFeed feed;

        using (StringReader reader = new StringReader(response)) {
            feed = (RssFeed)products.Deserialize(reader);
        }

        var count = feed == null ? 0 : feed.Channel.Products.Count;

        string countMessage = "Execute ran and the total count was: " + count + "(XML)";
        Console.WriteLine(countMessage);
        return countMessage;

    }
}

public interface IJob
{
    Task<string> Execute(
        JobArguments arguments,
        Func<string, Task> info,
        Func<string, Task> warn,
        CancellationToken token);
}
public class JobArguments
{
    public JobArguments(
        Guid datasetId,
        string apiKey,
        IReadOnlyDictionary<string, string> jobConfiguration)
    {
        DatasetId = datasetId;
        ApiKey = apiKey;
        JobConfiguration = jobConfiguration;
    }
    public Guid DatasetId { get; }
    public string ApiKey { get; }
    public IReadOnlyDictionary<string, string> JobConfiguration { get; }
}