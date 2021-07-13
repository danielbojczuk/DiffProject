# DiffProject
DiffProject is a REST API to check differences between two sets of Binary Data.

### Usage
Using a GUID as a unique key, at least three endpoints should need to be called. The first two endpoints set the *right* and *left* data to be compared. They could be called at any order. As soon the last data is set or updated, DiffProject will compare the data and save the result that could be retrieved in the third Endpoint.

**The data to be compared should be provided as a Base64 Encoded String.**

### Endpoints:

 **/v1/diff/{**Comparison ID**}/left** and **/v1/diff/{**Comparison ID**}/right**

| Method | Returns     | Description                     |
|--------|-------------|---------------------------------|
| POST   | 201 Created | Set a new data to be compared.  |
| PUT    | 200 Ok      | Update the data to be compared. |
| GET    | 200 Ok      | Retrieve the data.              |

All the resquests will return in body the Binary Data Object:

| Field            | Type   | Descroption                                |
|------------------|--------|--------------------------------------------|
| comparisonSide   | string | Provided comparison side "right" or "left" |
| base64BinaryData | string | Provided Base64 encoded Binary Data        |
| comparisonId     | UUID   | Provided comparison ID                     |
| id               | UUID   | Binary Data ID                             |



**/v1/diff/{**Comparison ID**}**

| Method | Returns     | Description                                  |
|--------|-------------|----------------------------------------------|
| GET    | 200 Ok      | Retrieve the comparison result.              |

The result contains 4 fields:

| Field       | Type                                  | Descroption                                                                                                          |
|-------------|---------------------------------------|----------------------------------------------------------------------------------------------------------------------|
| sidesEqual  | bool                                  | Indicates whether the data are equal or not                                                                          |
| sameSize    | bool                                  | Indicates whether the sides have the same size or not                                                                |
| differences | Collection<long position,long lenght> | If the sides aren't equal but have the same size it contains a list with the position and lenght of the differences. |
| id          | UUID                                  | Comparison Result ID                                                                                                 |

**Example:**

1 - Setting the left side:
>**Request**:
>
>curl -X POST "https://localhost:44367/v1/diff/a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5/left" -H  "accept: */*" -H  "Content-Type: application/json" -d "\"RGFuaWVsIEFudG9uaW8gRiBCb2pjenVr\""

>**Response**:
>
>*Status code*: 201
>
>*Body*: {"comparisonSide":"left","base64BinaryData":"RGFuaWVsIEFudG9uaW8gRiBCb2pjenVr","comparisonId":"a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5","id":"52f48417-f8d5-4075-b92c-8ae39baaffde"}

2 - Setting the right side:
>**Request**:
>
>curl -X POST "https://localhost:44367/v1/diff/a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5/right" -H  "accept: */*" -H  "Content-Type: application/json" -d "\"RGFuaWVsIEFudG9uaW8gRyBCb2pjenVr\""

>**Response**:
>
>*Status code*: 201
>
>*Body*: {"comparisonSide":"right","base64BinaryData":"RGFuaWVsIEFudG9uaW8gRyBCb2pjenVr","comparisonId":"a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5","id":"dbd2ae37-2ebc-4a5f-af71-1442edd30dfb"}

3 - Retrieving the result:
>**Request**:
>
>curl -X GET "https://localhost:44367/v1/diff/a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5" -H  "accept: */*"

>**Response**:
>
>*Status code*: 200
>
>*Body*: {"sidesEqual":false,"sameSize":true,"differences":{"15":1},"comparisonId":"a0a84aaa-f1e2-47c7-bb97-a3de2b6aa2d5"}

