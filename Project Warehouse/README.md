<h1 align="center">Project Warehouse </h1>

<h2>Project Description </h2>
<p>This is an API- Minimal that manages a warehouse system with different types of movements and many locations. In this app you can create several movements of different type of materials, add stroch of your materials and make shipments to your customers and reduce the stock of the shipment. You cn see all the movenets that happens in your Warehouse system to track them and make changes as you want.</p>

<h2> Our Database </h2>

![alt text](<ERD-PROJECT WAREHOUSE.png>)


<h2> Our Endpoints </h2>

<h5>- Reset-Transactions</h5>
This endpoint erase all information of your transaction on the database. This is used to start from 0 when you test the API

<h5>- list/materials</h5>
Enpoint that show you the list of all materials from the database.

<h5>- list/users</h5>
Enpoint that show you the list of all users from the database.

<h5>- list/locations</h5>
Enpoint that show you the list of all locations and their related bins.

<h5>- list/Movements</h5>
Enpoint that show you the list of all movements and their related materials that were move in every movement.

<h5>- list/BinMaterials/{Bin}</h5>
Endoint that shows you all the materials located in a specific bin.

<h5>- list/Shipments</h5>
Enpoint that show you the list of all shipmets and their related materials that were ship.

<h5>- AddStoch</h5>
Enpoint that gives stock to some material. It store the material stock in some bin and make the respective movement of the material.

<h5>- Move-Stock</h5>
Enpoint that move an extisting stock from one bin to another. It will check if the bin has enough stock to make the movement, oad if not will throw a custom exception (For demo purposes it is not catched). It also save the movement in the database.

<h5>- Make-Shipment</h5>
Enpoint that remove stock from some bin and store the shipment and a related movement. It also checks if you have enough stock, but if not it will return a BadRequest()

<h5>- Burst/Movements-Priority</h5>
Enpoint that generates a given number of movements to be processed, stores it into a PriorityQueue and execute its concurrently depending on the priority that it has. Finally log the number of operation successfull and the failed operations of each type and the time of excecution of the operetions.


<h2> Use of Technologies </h2>
- Use of EF Core model + migrations + seed data against SQL Server in Docker, DbContext in DI.<br>
- Minipal API endpoints (Get and Set) with different functionalitys.<br>
- Managing Burst fuction with task and concurrent collections, with every task with teir own dbcontext, and one transaction per order guaranted by a RowVersion.<br>
- Burst funcion run in background with task, making the API responsive during their execution.<br>
- Serilog structured logging.<br>
- Executing transactions with priority via PriorityQueue.<br>
- Function to reset transactions.<br>
- Benchmark of burst process logging the correspondin execution time.<br>
- ConcurrentDictionary lookups at stock checks.<br>
- Custom exception that carries data.<br>
