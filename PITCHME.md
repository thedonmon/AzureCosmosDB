@title[Introduction]

# Microsoft Azure Cosmos DB

### A ShipBob Presentation

#### Presented by

@llett & @dmaman

---

## Agenda
<ul>
  <li>What is CosmosDB?</li>
  <li>Why we use it</li>
  <li>Global Distribution</li>
  <ul>
  <li>Scaling & DTU usage</li>
  <li>Indexing</li>
  <li>Partitions</li>
  </ul>
  <li>Supports a galaxy of APIs and languages</li>
  <li>Configuration & Code Samples</li>
  <li>Lessons Learned</li>
</ul>
---

## What is Cosmos DB?

Azure Cosmos provides a globally distributed database system that leverages a multitude of database models. By combining the robust features of Azure and flexible SDK, you can implement a quick solution for any business problem. 

---

## Why We Use It
<ul>
<li>Ease of creating a databse almost instantaneously</li>
<li>Creating object models to be stored as is - without schemas!</li>
<li>Turnkey Distribution</li>
<li>Low-cost of ownership - pay for space and scale as you go!</li>
<li>Always available</li>
<li>Configurable connection policies</li>
</ul>
---

## Global Distribution
<ul>
  <li>Databases can be replicated on the fly to ensure data is always available</li>

<li>Provision throughput at the database level</li>

<li>Different databases can be set to different DTU usages</li>

<li>Partitions can live in their own domain accross the distributed network</li>
<li>Policy based geo-fencing (China & Germany)</li>

<li>Global writes without implementing a replication engine.</li>
</ul>
---

## How does Azure Cosmos DB index data?
@ul
- Azure Cosmos DB default indexing policy

  - Default uses Consistent Indexing - synchronous indexing as documents enter, they are indexed costly over large sets of data

  - Indexes over all fields and subfields of an object - also costly for large sets
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

```csharp
DocumentCollection myCollection = new DocumentCollection();
myCollection.Id = "collection";
myCollection.PartitionKey.Paths.Add("/deviceId");
```
@[3](Partition on a field when creating the collection)

```csharp

/*Read document. 
 *Needs the partition key and the ID to be specified 
*/
Document result = await client
.ReadDocumentAsync(UriFactory.
   CreateDocumentUri("database", "collection", "XMS-001-52C"),
   new RequestOptions {
    PartitionKey = new PartitionKey("XMS-0001")
 });

```
@[12](Set partition key via code to query on)

---

## API Support & Languages
@ul
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
@ulend
---

## Code Samples!

+++

```csharp
public async Task InitializeAsync() {
 client = new DocumentClient(
  new Uri(Endpoint), AccessKey, new ConnectionPolicy {
  EnableEndpointDiscovery = true
 });

 await client.CreateDatabaseIfNotExistsAsync(
 new Database() {
  Id = DatabaseId
 });
 await client.
   CreateDocumentCollectionIfNotExistsAsync(
   UriFactory.CreateDatabaseUri(DatabaseId), 
   new DocumentCollection {
      Id = CollectionId
   });
}

private async Task CreateDatabaseIfNotExistsAsync() {
 try {
  await client.ReadDatabaseAsync(
   UriFactory.CreateDatabaseUri(DatabaseId));
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
  await client.ReadDocumentCollectionAsync(
   UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
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
@[19-28](Creating a database through the SDK)
@[32-49](Creating the collection and throughput)

---

### Lessons Learned

- Size restrictions on documents being stored (currently 2MB)

- Leverage partitions!

---

### Questions?

<br>


