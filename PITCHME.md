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
<ul>
<li>Partitions can live in their own domain accross the distributed network</li>
<li>Policy based geo-fencing (China & Germany)</li>
</ul>
<li>Global writes without implementing a replication engine.</li>
</ul>
---

## How does Azure Cosmos DB index data?
<ul>
  <li>Azure Cosmos DB default indexing policy</li>
  <li>Customizing Indexes</li>
  <li>Can be customized via code or through Azure portal!</li>
</ul>
![Logo](https://media.giphy.com/media/1Aj18QFr0xz5LwrTRq/giphy.gif)
---

## Partitioning Collections
<ul>
  <li>Partition Key are defined via the path on the JSON object</li>

  <li>Partition Key syntax vs the indexing syntax - partitioned paths cannot be excluded from indexing policies</li>

  <li>Enables parallelism - searching multiple partitions in parallel</li>

  <li>Also enables sproc support</li>
</ul>
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
@[8-12](Set partition key via code to query on)

---

## API Support & Languages
<ul>
<li>Microsoft finally open source?!</li>

<li>Supports - .NET, Python, Node.js, Java, and Xamarin (mobile apps) </li>
  
<li>SQL API</li>

<li>Table API</li>

<li>Gremlin API</li>

<li>MongoDB API</li>

<li>Cassandra API</li>
</ul>
---

## Configuration With Code!

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

### DEMO & Code Samples

---
### Lessons Learned

- Size restrictions on documents being stored (currently 2MB)

- Leverage partitions!

---

### Questions?

<br>


