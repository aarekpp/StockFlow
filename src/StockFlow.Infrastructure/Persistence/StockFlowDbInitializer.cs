using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StockFlow.Domain.Entities;
using StockFlow.Domain.Enums;

namespace StockFlow.Infrastructure.Persistence;

public static class StockFlowDbInitializer
{
    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<StockFlowDbContext>();
        await SeedAsync(context, cancellationToken);
    }

    public static async Task SeedAsync(StockFlowDbContext context, CancellationToken cancellationToken = default)
    {
        if (await context.Categories.AnyAsync(cancellationToken)) return;

        var now = DateTime.UtcNow;

        var electronics = new Category("Electronics", "Consumer electronics and accessories");
        var office = new Category("Office supplies", "Paper, stationery and small office equipment");
        var tools = new Category("Tools", "Hand and power tools");
        var cleaning = new Category("Cleaning", "Cleaning products and consumables");

        var laptop = new Product("Business laptop 15-inch", 3499.00m, "pcs", "15-inch laptop for office work", electronics.Id);
        var mouse = new Product("Wireless mouse", 59.90m, "pcs", "2.4 GHz wireless optical mouse", electronics.Id);
        var paper = new Product("A4 copy paper (500 sheets)", 22.50m, "ream", null, office.Id);
        var drill = new Product("Cordless drill 18V", 349.00m, "pcs", "Cordless drill with two batteries", tools.Id);
        var cleaner = new Product("Multi-surface cleaner 1L", 14.99m, "bottle", null, cleaning.Id);

        Receive(laptop, 12, now);
        Receive(mouse, 8, now);
        Receive(paper, 150, now);
        Receive(drill, 3, now);
        Receive(cleaner, 2, now);

        var techData = new Supplier("TechData Wholesale", "ul. Elektroniczna 1, 00-001 Warszawa", "+48 000 000 001", "sales@techdata.example", "1000000001");
        var officeWorld = new Supplier("OfficeWorld", "ul. Biurowa 2, 00-002 Warszawa", "+48 000 000 002", "orders@officeworld.example", "1000000002");
        var toolMaster = new Supplier("ToolMaster", "ul. Narzędziowa 3, 00-003 Warszawa", "+48 000 000 003", "contact@toolmaster.example", "1000000003");
        var cleanPro = new Supplier("CleanPro", "ul. Czysta 4, 00-004 Warszawa", "+48 000 000 004", "hello@cleanpro.example");

        laptop.AddSupplier(techData.Id, 2900.00m, 5);
        mouse.AddSupplier(techData.Id, 35.00m, 3);
        paper.AddSupplier(officeWorld.Id, 15.00m, 2);
        drill.AddSupplier(toolMaster.Id, 240.00m, 7);
        drill.AddSupplier(techData.Id, 260.00m, 4);
        cleaner.AddSupplier(cleanPro.Id, 8.00m, 2);

        var anna = new Customer(CustomerType.Person, "Anna Kowalska", "ul. Lipowa 5, 00-101 Warszawa", "anna.kowalska@example.com", "+48 111 111 111");
        var jan = new Customer(CustomerType.Person, "Jan Nowak", "ul. Dębowa 7, 00-102 Kraków", "jan.nowak@example.com", "+48 111 111 112");
        var budInstal = new Customer(CustomerType.Company, "Bud-Instal Sp. z o.o.", "ul. Budowlana 10, 00-201 Łódź", "biuro@budinstal.example", "+48 222 222 221", "2000000001");
        var techSoft = new Customer(CustomerType.Company, "TechSoft S.A.", "ul. Programistów 12, 00-202 Wrocław", "office@techsoft.example", "+48 222 222 222", "2000000002");

        var order = new Order(budInstal.Id, now, "Sample order created by the seed");
        order.AddItem(drill, 2);
        order.AddItem(paper, 10);

        context.Categories.AddRange(electronics, office, tools, cleaning);
        context.Suppliers.AddRange(techData, officeWorld, toolMaster, cleanPro);
        context.Customers.AddRange(anna, jan, budInstal, techSoft);
        context.Products.AddRange(laptop, mouse, paper, drill, cleaner);
        context.Orders.Add(order);

        await context.SaveChangesAsync(cancellationToken);
    }

    private static void Receive(Product product, int quantity, DateTime occurredAt)
    {
        product.RegisterStockMovement(StockMovementType.Receipt, quantity, occurredAt, comment: "Initial stock (seed)");
    }
}
