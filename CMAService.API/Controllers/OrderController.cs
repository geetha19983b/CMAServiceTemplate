using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using Newtonsoft.Json;
using CMAService.API;
namespace CMAService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ProducerConfig producerConfig;

        private readonly ConsumerConfig consumerconfig;
        public OrderController(ProducerConfig producerConfig, ConsumerConfig consumerConfig)
        {
            this.producerConfig = producerConfig;
            this.consumerconfig = consumerConfig;

        }
        // POST api/values
        

        //{

        //"id":1236,
        //"productname":"Order 1",
        //"quantity":3

        //}
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody]OrderRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Serialize 
            string serializedOrder = JsonConvert.SerializeObject(value);

            Console.WriteLine("========");
            Console.WriteLine("Info: OrderController => Post => Recieved a new purchase order:");
            Console.WriteLine(serializedOrder);
            Console.WriteLine("=========");

            var producer = new ProducerWrapper(this.producerConfig, "orderrequests");
            await producer.writeMessage(serializedOrder);

            return Created("TransactionId", "Your order is in progress");
        }
        [HttpGet]
        [Route("ProcessOrder")]
        public async Task<ActionResult> ProcessMessage()
        {
            var consumerHelper = new ConsumerWrapper(consumerconfig, "orderrequests");
            List<string> lstorderRequests = consumerHelper.readAllMessages();
            if (lstorderRequests.Any())
            {
                foreach (var orderRequest in lstorderRequests)
                {
                    //TODOprocess the order
                }

                return Ok(lstorderRequests);
            }
            return Ok("No messages to process");

        }

    }


    public class OrderRequest
    {
        public int id { get; set; }
        public string productname { get; set; }
        public int quantity { get; set; }

        public OrderStatus status { get; set; }
    }
    public enum OrderStatus
    {
        IN_PROGRESS,
        COMPLETED,
        REJECTED
    }
}
