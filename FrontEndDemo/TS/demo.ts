// Lets cerate an actual program - eventually 
// this will hit our API
// For now, we can use static data 

// Notice we import from a .js file - that's the runtime path
// because what actually runs is /dist/demo.js
import {InventoryItem, HttpStatus, SortDirection, SuppierPrice,
    FetchState, Sku, InventoryPatch, NewInventoryItem
}  from "./types.js";
import { ApiClient, isApiError } from "./ts-client.js";

// For now, lets create what our API will hand back to us
let catalog: InventoryItem[] = []; // starting with an empty array now
let catalogState: FetchState = "idle"; // starting out, our client is chilling

// AS CONST - the  values become literal types and the object is frozen at the type level.
// reassigning to something declared with as const is a compile error
const MESSAGES = {
    signIn: "sign in to see supplier prices",
    empty: "the catalog is empty"
} as const;

// AS CONST - a runtime array who's literals become a union type
const SORT_KEYS = ["name", "sku", "currentStock"] as const;
type SortKey = typeof SORT_KEYS[number]; // "name" | "sku" | "currentStock"

// Lets write another guard - proving a value passed in corresponds to something
// inside SortKey
function isSortKey(value: string): value is SortKey {
    if (!(SORT_KEYS as readonly string[]).includes(value)) return false;
    return true;
}

// KEYOF + GENERICS  - Lets write a function to sort by multiple fields
// K extends keyof InventoryItem - allows us to take in fields of InventpryItem
// This lets me check at compile time that I'm passing a valid value for K
// sortBy(items, "price", "asc") - this would be a compile error, because that second argument
// doesn't correspont to a field/property of InventoryItem
function sortBy<K extends keyof InventoryItem>(items: InventoryItem[], 
    key: K, dir: SortDirection): InventoryItem[]{
        //1. Determine wheter to sort Ascending or Descending
        const isDescending = dir === SortDirection.Descending; // true or false

        // 2. Create a fresh copy of the arrat to avoid mutating the original
        const arrayCopy = [...items]; // rest operator

        // 3. Apply the built in sorting algorithm - we provide the logic
        arrayCopy.sort((itemA, itemB) => {
            // 4. Extract the values using that key that was passed in
            const valueA = itemA[key];
            const valueB = itemB[key];

            // 5. Handle numeric properties
            if(typeof valueA === "number" && typeof valueB === "number"){
                if (isDescending) return valueB - valueA;
                else return valueA - valueB;
            }
            
            // 6. If we made it here, they are string properties
            const stringA = String(valueA);
            const stringB = String(valueB);

            if(isDescending){
                // using localeCompare (a built in string method) to compare the stringd
                // localeCompare returns -1 0 or 1 based on alphabetical order of the first character
                // of the strings
                return stringB.localeCompare(stringA);
            }
            else return stringA.localeCompare(stringB);
        });

        return arrayCopy; // return that sorted array copu
    }

    function prinSorted(key: SortKey, dir: SortDirection): void
{
    console.log(`\nsorted by ${key} ${dir}:`);
    printCatalog(sortBy(catalog, key, dir));
}
// For now, lets create what our API will hand back to us
// const catalog: InventoryItem[] = [
//     {sku: "Bk-001", name: "Clean Code", currentStock: 5},
//     {sku: "Bk-002", name: "Dune", currentStock: 3},
//     {sku: "Bk-003", name: "Refactoring", currentStock: 8}
// ];

// Lets see our first TS function
// We type argumenys and we type the return
function printCatalog(items: InventoryItem[]): void{

    // Alerting user of pur catalog is empty
    if (items.length === 0){
        console.log(MESSAGES.empty);
        return;
    }

    for (const item of items){
        console.log(`${item.sku} ${item.name} ${item.currentStock}`);
    }
}

// Load catalog
async function loadCatalog(): Promise<void> {
    catalogState = "loading";
    console.log("loading catalog...");

    // Calling our API
    const result = await api.getJson<InventoryItem[]>("/api/Inventory");

    // using oir type guard - checking if we got an ApiError
    if (isApiError(result)){
        catalogState = "failed"; // failed to get catalog state, got an ApiError
        console.log(result.message);
        return;
    }
}

// Show supplier price
async function showSupplierPrice(sku: Sku): Promise<void> {
    // Call our api
    const result = await api.getJson<SuppierPrice>(`/api/Inventory/${sku}/supplier-price`);

    // using that type guard again
    if (isApiError(result)){
        // Lets handle a 401 versus all other possible codes
        if(result.status === HttpStatus.Unauthorized){
            console.log(MESSAGES.signIn);
        }
        else{ 
            console.log(result.message);
        }
        
        return;
    }

    console.log(`Supplier list at ${result.supplierPrice}`);
}

console.log("catalog: ");
printCatalog(catalog);

// Enums are RUNTIME objects - interfaces and aliases are NOT
// In TS numeric enums actually map both ways
console.log(HttpStatus.Unauthorized); // 401
console.log(HttpStatus[401]); // Unauthorized

// Lets use our client
const api = new ApiClient();

const liveCatalog = await api.getJson("/api/Inventory");

console.log(liveCatalog);