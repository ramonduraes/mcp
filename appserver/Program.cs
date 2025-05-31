using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Text.Json;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using Microsoft.AspNetCore.Builder; 
using Microsoft.Extensions.DependencyInjection;



Console.WriteLine("🚀 Initializing MCP Server...");
Console.WriteLine("http://localhost:5000/mcp");
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
            .WithHttpTransport()
            .WithToolsFromAssembly();


var app = builder.Build();
app.MapGet("/", () => "✅ MCP Server is running");
app.MapMcp("/mcp");
app.Run();

Console.WriteLine("🛑 MCP Server shutdown.");


[McpServerToolType]
[Description("Creates a customer order with structured items.")]
public class CreateOrderTool 
{
    public string Name => "CreateOrder";
    public string Description => "Creates a customer order with structured items.";
    [McpServerTool(Name = "order.add"), Description("Creates a new order.")]
    public Task<object> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken = default)
    {
        var result = new
        {
            message = $"✅ Order from {input.CustomerName} ({input.CustomerTaxId}) received with {input.Items.Count} items. Total: ${input.Total}"
        };

        return Task.FromResult<object>(result);
    }
}


public class CreateOrderInput
{
    [JsonPropertyName("customerName")]
    [Description("Full name of the customer placing the order.")]
    public string CustomerName { get; set; } = string.Empty;

    [JsonPropertyName("customerTaxId")]
    [Description("Tax number of the customer.")]
    public string CustomerTaxId { get; set; } = string.Empty;

    [JsonPropertyName("items")]
    [Description("List of items included in the order.")]
    public List<OrderItemInput> Items { get; set; } = new();

    [JsonPropertyName("total")]
    [Description("Total monetary value of the entire order.")]
    public decimal Total { get; set; }
}

public class OrderItemInput
{
    [JsonPropertyName("productId")]
    [Description("Unique identifier of the product.")]
    public string ProductId { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    [Description("Quantity of the product to be ordered.")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    [Description("Unit price of the product.")]
    public decimal Price { get; set; }
}


