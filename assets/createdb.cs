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
