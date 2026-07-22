import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getInventoryItem } from "../api/inventory";
import type { InventoryItem, FetchState } from "../types";

// BookDetai; will use a route with a URL parameter - useParams reads that `:sku` from the path
export function BookDetail() {
    // grabbing sku from URL path
    const {sku} = useParams<{ sku: string}>();
    const [item, setItem] = useState<InventoryItem | null>(null);
    const [FetchState, setFState] = useState<FetchState>("idle");

    // useEffect - this time we have a dependency. The effect (the api call we make)
    // depends on "sku" - if a user navigates at different sku, useEffect re-runs
    useEffect(() =>{
        if (!sku) return;

        let active = true;
        setFState("loading");

        getInventoryItem(sku).then((data) => {
            if (!active) return;
            setItem(data);
            setFState("loaded");
        }).catch(() => {
            if (active) setFState("failed");
        })

        return () => {
            active = false;
        };
    }, [sku]);

    if (FetchState === "idle" || FetchState === "loading" ) return <p>Loading...</p>

    if (FetchState == "failed" || !item)
        return (
            <p>
                Book {sku} not found. <Link to="/">&larr: Back to catalog</Link>
            </p>
        );

    return (
        <article>
            <p>
                <Link to="/"> Back to the catalog</Link>
            </p>
            <h2>{item.name}</h2>
            <p>Sku: {item.sku}</p>
            <p>In Stock: {item.currentStock}</p>
        </article>
    )
}