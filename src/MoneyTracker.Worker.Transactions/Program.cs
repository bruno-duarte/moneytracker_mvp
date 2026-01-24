using MoneyTracker.Application;
using MoneyTracker.Application.Messaging.Handlers;
using MoneyTracker.Application.Messaging.Topics;
using MoneyTracker.Domain.Events;
using MoneyTracker.Infrastructure;
using MoneyTracker.Messaging.Kafka.Configuration;
using MoneyTracker.Messaging.Kafka.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Application
builder.Services.AddApplication(builder.Configuration);

// Kafka
builder.Services.AddKafkaMessaging(options =>
    builder.Configuration
        .GetSection("Kafka")
        .Bind(options));

builder.Services.AddKafkaDeadLetterPublisher();

builder.Services.AddKafkaConsumer<TransactionCreatedEvent, TransactionCreatedHandler>(
    new KafkaConsumerOptions
    {
        Topic = TransactionTopics.Created,
        GroupId = "moneytracker-transactions-worker-dev5",
        DeadLetterTopic = "transaction.created.dlq",
        EnableAutoCommit = false
    });

var host = builder.Build();
await host.RunAsync();

