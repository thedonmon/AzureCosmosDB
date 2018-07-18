// Read document. Needs the partition key and the ID to be specified 
Document result = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri("db", "coll", "XMS-001-FE24C"),
 new RequestOptions {
  PartitionKey = new PartitionKey("XMS-0001")
 });
