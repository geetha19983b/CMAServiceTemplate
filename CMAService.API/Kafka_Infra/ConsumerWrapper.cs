using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
namespace CMAService.API
{
    public class ConsumerWrapper
    {
        private string _topicName;
        private ConsumerConfig _consumerConfig;
        private IConsumer<string, string> _consumer;
        private static readonly Random rand = new Random();
        public ConsumerWrapper(ConsumerConfig config, string topicName)
        {
            this._topicName = topicName;
            this._consumerConfig = config;
            this._consumerConfig.AutoOffsetReset = AutoOffsetReset.Earliest;
            this._consumer = new ConsumerBuilder<string, string>(this._consumerConfig).Build();
           
        }
        public List<string> readAllMessages()
        {
            
            List<string> msgLst = new List<string>();

            try
            {

                
                TopicPartitionOffset tps = new TopicPartitionOffset(new TopicPartition(this._topicName, 0), Offset.Beginning);
                this._consumer.Assign(tps);

                while (true)
                {
                    var result = this._consumer.Consume();
                    if (result == null || result.IsPartitionEOF)
                    {
                        Console.WriteLine("No messages...");
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Offset: {result.Offset}");
                        msgLst.Add(result.Value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                this._consumer.Close();
            }
            return msgLst;
        }
    }
}
