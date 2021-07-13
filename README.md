# DiffProject
Is a REST API chech where are de differences between two sets of Binary Data.

### Usage
Using a GUID as a unique Key you will have to call at least three endpoints. The fist two endpoins mus be to set the *right* and *left* data to compare. They can be set at any order. As soon the last data is set or updated, the Diff Project will compare the data and save te result to be retrieved in the third Endpoint.
The result contains 4 fields:
* sidesEqual: Indicates whether the sides are equal or not. 
* samseSize: Indicates whether the sides have the same size or not.
* differences: If the sides aren't equal but have the seme size it contains a list wi the position and size of the difference.
* comparisonId: The provided Comparison ID.

**The data to be compared should be provided as a Base64 Encoded String.**

### Endpoints:

 * /v1/diff/**Comparison ID**/left
   * POST: Set a new data to be compared. I 
   * PUT: Update the data to be compared.
   * GET: Retrieve the data.

* /v1/diff/**Comparison ID**/right
   * POST: Set a new data to be compared.
   * PUT: Update the data to be compared.
   * GET: Retrieve the data.  

* /v1/diff/**Comparison ID**
  * GET: Retrieve the Comparison Result. 

Example:

**Setting the left side:**
>**Request**:
>
>curl -X POST "https://localhost:44367/v1/diff/a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5/left" -H  "accept: */*" -H  "Content-Type: application/json" -d "\"RGFuaWVsIEFudG9uaW8gRiBCb2pjenVr\""

>**Response**:
>
>*Status code*: 201
>
>*Body*: {"comparisonSide":"left","base64BinaryData":"RGFuaWVsIEFudG9uaW8gRiBCb2pjenVr","comparisonId":"a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5","id":"52f48417-f8d5-4075-b92c-8ae39baaffde"}

**Setting the right side:**
>**Request**:
>
>curl -X POST "https://localhost:44367/v1/diff/a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5/right" -H  "accept: */*" -H  "Content-Type: application/json" -d "\"RGFuaWVsIEFudG9uaW8gRyBCb2pjenVr\""

>**Response**:
>
>*Status code*: 201
>
>*Body*: {"comparisonSide":"right","base64BinaryData":"RGFuaWVsIEFudG9uaW8gRyBCb2pjenVr","comparisonId":"a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5","id":"dbd2ae37-2ebc-4a5f-af71-1442edd30dfb"}

**Retrieving the result:**
>**Request**:
>
>curl -X GET "https://localhost:44367/v1/diff/a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5" -H  "accept: */*"

>**Response**:
>
>*Status code*: 200
>
>*Body*: {"sidesEqual":false,"sameSize":true,"differences":{"15":1},"comparisonId":"a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5"}

