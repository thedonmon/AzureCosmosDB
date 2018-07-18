@title[Introduction]

# Microsoft Azure Cosmos DB

### A ShipBob Presentation

#### Presented by

<br>

@llett & @dmaman

---

## Agenda
@ul
- What is CosmosDB? 
  - Why we use it

  - Global Distribution

    - Scaling & DTU usage

    - Indexing

    - Partitions

  - Supports a galaxy of APIs and languages
- Configuration & Code Samples 
  - Project: AnalyzeMyStore
- Lessons Learned 
@ulend
---

## What is Cosmos DB?

Azure Cosmos provides a globally distributed database system that leverages a multitude of database models. By combining the robust features of Azure and flexible SDK, you can implement a quick solution for any business problem. 

---

## Why We Use It
@ul
- Ease of creating a databse almost instantaneously 

- Creating object models to be stored as is - without schemas!

- Turnkey Distribution

- Low-cost of ownership - pay for space and scale as you go!

- Always available (money back guarantees)\

- Configurable connection policies
@ulend
---

## Global Distribution
@ul
- Databases can be replicated on the fly to ensure data is always available

- Provision throughput at the database level

  - Different databases can be set to different DTU usages - NOTE: get gif of scaling the DTU usage

  - Partitions can live in their own domain accross the distributed network 
@ulend
@ul
- Policy based geo-fencing (China & Germany)

- Global writes without implementing a replication engine. 
@ulend
---

## How does Azure Cosmos DB index data?
@ul
- Azure Cosmos DB default indexing policy

  - Default uses Consistent Indexing - synchronous indexing as documents enter, they are indexed costly over large sets of data

  - Indexes over all fields and subfields of an object - also costly for large sets
@ulend
@ul
- Customizing Indexes

  - Choose which paths to include or exclude from indexing

  - Configure indexing types - hash, range, numeric, string, precision etc.

  - Configure indexing strategy - Consistent vs. Lazy vs. None

- Can be customized via code or through Azure portal! - add screenshot
@ulend
---

## Partitioning Collections
@ul
- Partition Key are defined via the path on the JSON object

- Partition Key syntax vs the indexing syntax - partitioned paths cannot be excluded from indexing policies

- Enables parallelism - searching multiple partitions in parallel

  - Also enables sproc support
@ulend
+++
---?code=assets/partition.cs&lang=csharp&title = Partition Through Code
```csharp

// Read document. Needs the partition key and the ID to be specified 
Document result = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri("db", "coll", "XMS-001-FE24C"),
 new RequestOptions {
  PartitionKey = new PartitionKey("XMS-0001")
 });

```
@[6](Set partition key via code)
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

+++

```csharp
        public async Task InitializeAsync() {
         client = new DocumentClient(new Uri(Endpoint), AccessKey, new ConnectionPolicy {
          EnableEndpointDiscovery = true
         });

         await client.CreateDatabaseIfNotExistsAsync(new Database() {
          Id = DatabaseId
         });
         await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(DatabaseId), new DocumentCollection {
          Id = CollectionId
         });
        }

        private async Task CreateDatabaseIfNotExistsAsync() {
         try {
          await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
         } catch (DocumentClientException e) {
          if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
           await client.CreateDatabaseAsync(new Database {
            Id = DatabaseId
           }).ConfigureAwait(false);
          } else {
           throw;
          }
         }
        }


        private async Task CreateCollectionIfNotExistsAsync() {
         try {
          await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
         } catch (DocumentClientException e) {
          if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
           await client.CreateDocumentCollectionAsync(
            UriFactory.CreateDatabaseUri(DatabaseId),
            new DocumentCollection {
             Id = CollectionId
            },
            new RequestOptions {
             OfferThroughput = 1000
            });
          } else {
           throw;
          }
         }
        }
```

@[1-4](DocumentClient should be a Singleton Instance)
@[14-24](Creating a database through the SDK)
@[28-40](Creating the collection and throughput)

---

### Lessons Learned

- Size restrictions on documents being stored (currently 2MB)

- Leverage partitions!

---

### Questions?

<br>


