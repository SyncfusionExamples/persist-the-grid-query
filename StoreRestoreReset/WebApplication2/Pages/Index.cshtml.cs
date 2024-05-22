using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Syncfusion.EJ2.Base;
using System.Collections;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        public System.Collections.Generic.List<OrdersDetails> Datasource { get; set; }
        private readonly ILogger<IndexModel> _logger;
        public static string gridState;
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Datasource = OrdersDetails.GetAllRecords().ToList();
        }

      
        public IActionResult OnPostStorePersistData([FromBody] StoreData persistData)
        {
            gridState = persistData.persistData;
            return new JsonResult(gridState);
        }

        public class StoreData
        {
            public string? persistData { get; set; }
        }
        public IActionResult OnGetRestore()
        {
            return Content(gridState);
        }
        public JsonResult Update([FromBody] CRUDModel<OrdersDetails> value)
        {
            //var Order = OrdersDetails.GetAllRecords();
            var data = OrdersDetails.GetAllRecords().Where(or => or.OrderID == value.Value.OrderID).FirstOrDefault();
            if (data != null)
            {
                data.OrderID = value.Value.OrderID;
                data.EmployeeID = value.Value.EmployeeID;
                data.CustomerID = value.Value.CustomerID;
                data.Freight = value.Value.Freight;
                data.OrderDate = value.Value.OrderDate;
                data.ShipCity = value.Value.ShipCity;
                data.Verified = value.Value.Verified;
            }

            return new JsonResult(new { value.Value });
        }
        public JsonResult OnPostInsert([FromBody] CRUDModel<OrdersDetails> value)
        {
            var Order = OrdersDetails.GetAllRecords();
            var obj = OrdersDetails.GetAllRecords().Where(or => or.OrderID.Equals(int.Parse(value.Value.OrderID.ToString()))).FirstOrDefault();
            OrdersDetails.GetAllRecords().Insert(0, value.Value);
            return new JsonResult(new { value.Value });
        }
        public JsonResult OnPostRemove([FromBody] CRUDModel<OrdersDetails> value)
        {
            var data = OrdersDetails.GetAllRecords().Where(or => or.OrderID.Equals(int.Parse(value.Key.ToString()))).FirstOrDefault();
            OrdersDetails.GetAllRecords().Remove(data);
            return new JsonResult(value);
        }

        public JsonResult OnPostUrlDatasource([FromBody] DataManagerRequest dm)
        {
            IEnumerable<OrdersDetails> DataSource = OrdersDetails.GetAllRecords().AsEnumerable();
            DataOperations operation = new DataOperations();
            List<string> str = new List<string>();
            if (dm.Aggregates != null)
            {
                for (var i = 0; i < dm.Aggregates.Count; i++)
                    str.Add(dm.Aggregates[i].Field);
            }
            IEnumerable aggregate = operation.PerformSelect(DataSource, str);

            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<OrdersDetails>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? new JsonResult(new { result = DataSource, count = count }) : new JsonResult(DataSource);
        }

        public JsonResult OnPostBatchUpdate([FromBody] CRUDModel<OrdersDetails> batchmodel)
        {
            if (batchmodel.Changed != null)
            {
                for (var i = 0; i < batchmodel.Changed.Count(); i++)
                {
                    var ord = batchmodel.Changed[i];
                    OrdersDetails val = OrdersDetails.GetAllRecords().Where(or => or.OrderID == ord.OrderID).FirstOrDefault();
                    val.OrderID = ord.OrderID;
                    val.EmployeeID = ord.EmployeeID;
                    val.ShipCity = ord.ShipCity;
                    val.ShipCountry = ord.ShipCountry;
                }
            }

            if (batchmodel.Deleted != null)
            {
                for (var i = 0; i < batchmodel.Deleted.Count(); i++)
                {
                    OrdersDetails.GetAllRecords().Remove(OrdersDetails.GetAllRecords().Where(or => or.OrderID == batchmodel.Deleted[i].OrderID).FirstOrDefault());
                }
            }

            if (batchmodel.Added != null)
            {
                for (var i = 0; i < batchmodel.Added.Count(); i++)
                {
                    OrdersDetails.GetAllRecords().Insert(0, batchmodel.Added[i]);
                }
            }
            return new JsonResult(new { added = batchmodel.Added, changed = batchmodel.Changed, deleted = batchmodel.Deleted, value = batchmodel.Value, action = batchmodel.Action, key = batchmodel.Key });

        }
    }
}

public class OrdersDetails
{
    public static List<OrdersDetails> order = new List<OrdersDetails>();
    public OrdersDetails()
    {

    }
    public OrdersDetails(int OrderID, string CustomerId, int EmployeeId, double Freight, bool Verified, DateTime OrderDate, string ShipCity, string ShipName, string ShipCountry, DateTime ShippedDate, string ShipAddress)
    {
        this.OrderID = OrderID;
        this.CustomerID = CustomerId;
        this.EmployeeID = EmployeeId;
        this.Freight = Freight;
        this.ShipCity = ShipCity;
        this.Verified = Verified;
        this.OrderDate = OrderDate;
        this.ShipName = ShipName;
        this.ShipCountry = ShipCountry;
        this.ShippedDate = ShippedDate;
        this.ShipAddress = ShipAddress;
    }
    public static List<OrdersDetails> GetAllRecords()
    {
        if (order.Count() == 0)
        {
            int code = 10000;
            for (int i = 1; i < 5; i++)
            {
                order.Add(new OrdersDetails(code + 1, "ALFKI", i + 0, 2.3 * i, false, new DateTime(2021, 11, 15), "Berlin", "Simons bistro", "Denmark", new DateTime(2021, 11, 16), "Kirchgasse 6"));
                order.Add(new OrdersDetails(code + 2, "ANATR", i + 2, 3.3 * i, true, new DateTime(2021, 11, 04), "Madrid", "Queen Cozinha", "Brazil", new DateTime(2021, 11, 11), "Avda. Azteca 123"));
                order.Add(new OrdersDetails(code + 3, "ANTON", i + 1, 4.3 * i, true, new DateTime(2021, 11, 30), "Cholchester", "Frankenversand", "Germany", new DateTime(2021, 11, 7), "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"));
                order.Add(new OrdersDetails(code + 4, "BLONP", i + 3, 5.3 * i, false, new DateTime(2021, 11, 22), "Marseille", "Ernst Handel", "Austria", new DateTime(2021, 11, 30), "Magazinweg 7"));
                order.Add(new OrdersDetails(code + 5, "BOLID", i + 4, 6.3 * i, true, new DateTime(2021, 11, 18), "Tsawassen", "Hanari Carnes", "Switzerland", new DateTime(2021, 11, 3), "1029 - 12th Ave. S."));
                code += 5;
            }
        }
        return order;
    }

    public int? OrderID { get; set; }
    public string CustomerID { get; set; }
    public int? EmployeeID { get; set; }
    public double? Freight { get; set; }
    public string ShipCity { get; set; }
    public bool Verified { get; set; }
    public DateTime OrderDate { get; set; }

    public string ShipName { get; set; }

    public string ShipCountry { get; set; }

    public DateTime ShippedDate { get; set; }
    public string ShipAddress { get; set; }
}
