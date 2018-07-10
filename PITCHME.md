@title[Introduction]

# Microsoft Azure Cosmos DB

### A ShipBob Presentation

#### Presented by

<br>

@llett & @dmaman

---

## Agenda

- What is CosmosDB? |
  - Why we use it

  - Global Distribution

    - Scaling & DTU usage

    - Indexing

    - Partitions

  - Supports a galaxy of APIs and languages
- Configuration & Code Samples |
  - Project: AnalyzeMyStore
- Lessons Learned |

---

## What is Cosmos DB?

Azure Cosmos provides a globally distributed database system that leverages a multitude of database models. By combining the robust features of Azure and flexible SDK, you can implement a quick solution for any business problem. 

---

## Why We Use It

- Ease of creating a databse almost instantaneously 

- Creating object models to be stored as is - without schemas!

- Turnkey Distribution

- Low-cost of ownership - pay for space and scale as you go!

- Always available (money back guarantees)\

- Configurable connection policies

---

## Global Distribution

- Databases can be replicated on the fly to ensure data is always available

- Provision throughput at the database level

  - Different databases can be set to different DTU usages - NOTE: get gif of scaling the DTU usage

  - Partitions can live in their own domain accross the distributed network 

- Policy based geo-fencing (China & Germany)

- Global writes without implementing a replication engine. 

---

## How does Azure Cosmos DB index data?

- Azure Cosmos DB default indexing policy

  - Default uses Consistent Indexing - synchronous indexing as documents enter, they are indexed costly over large sets of data

  - Indexes over all fields and subfields of an object - also costly for large sets

- Customizing Indexes

  - Choose which paths to include or exclude from indexing

  - Configure indexing types - hash, range, numeric, string, precision etc.

  - Configure indexing strategy - Consistent vs. Lazy vs. None

- Can be customized via code or through Azure portal! - add screenshot

---

## Partitioning Collections

- Partition Key are defined via the path on the JSON object

- Partition Key syntax vs the indexing syntax - partitioned paths cannot be excluded from indexing policies

- https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-partition-data get example from C# code 

- Enables parallelism - searching multiple partitions in parallel

  - Also enables sproc support

---

## API Support & Languages

- Microsoft finally open source?!

  - Supports - .NET, Python, Node.js, Java, and Xamarin (mobile apps) 

- SQL API

  - Closest syntax to SQL Server, rapid development of non-relational document databses. Microsoft NoSQL solution

- Table API

  - Migrate data from Azure Table Storage to Cosmos DB. Fully integrated with Azure SDKs

- Gremlin API

  - Leverage Gremlin for implementing and querying graph databases with Cosmos DB

- MongoDB API

  - Migrate existing MongoDB implementations to Cosomos DB. Supports the MongoDB aggregation pipeline

- Cassandra API

  - Integrate with Apache Cassandra API and query data from Cosmos DB using Cassandra Query Based Language tools. 

---

## Code Samples!

```csharp
        public async Task InitializeAsync()
        {
            client = new DocumentClient(new Uri(Endpoint), AccessKey, new ConnectionPolicy { EnableEndpointDiscovery = true });

            await client.CreateDatabaseIfNotExistsAsync(new Database() { Id = DatabaseId });
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseId), new DocumentCollection { Id = CollectionId });
        }
        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId }).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await     client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
@[3-4](DocumentClient should be a Singleton Instance)
```







---

### Questions?

<br>

@fa[twitter gp-contact](@gitpitch)

@fa[github gp-contact](gitpitch)

@fa[medium gp-contact](@gitpitch)

---?image=assets/image/gitpitch-audience.jpg

DISCUSSION NOTES:

-Indexing

- It's important to understand that when you manage indexing policy, you can make fine-grained trade-offs between index storage overhead, write and query throughput, and query consistency. - https://docs.microsoft.com/en-us/azure/cosmos-db/indexing-policies

| Consistency       | Indexing Mode: Consistent | Indexing Mode: Lazy | Indexing Mode: None |
| ----------------- | ------------------------- | ------------------- | ------------------- |
| Strong            | Strong                    | Eventual            | Strong              |
| Bounded staleness | Bounded staleness         | Eventual            | Bounded staleness   |
| Session           | Session                   | Eventual            | Session             |
| Eventual          | Eventual                  | Eventual            | Eventual            |

- Partitions

  - Should not be an afterthought behind indexing. Should probably be considered first before deciding indexing policies and paths

Sample Azure Cosmos Code

https://github.com/Azure/azure-documentdb-dotnet
