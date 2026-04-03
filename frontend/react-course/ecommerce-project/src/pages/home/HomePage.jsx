import axios from 'axios'
import { Header } from '../../components/Header';
import { ProductsGrid } from './ProductGrid';
import { useSearchParams } from 'react-router';
import './HomePage.css';

import { useEffect, useState } from 'react';

export function HomePage({ cart , loadCart }) {

    const [products, setProducts] = useState([]);
    const [searchParams]=useSearchParams();
    const search=searchParams.get('search')

    useEffect(() => {
        const getHomeData = async () => {
            if(!search)
            {
                const response = await axios.get('/api/products')
               setProducts(response.data);
            }
            else
            {
                const response = await axios.get(`/api/products?search=${search}`)
                setProducts(response.data);
            }
          
        }

        getHomeData();
    }, [search])


    return (
        <>
            <title>HomePage</title>
            <link rel="icon" type="image/svg+xml" href="/images/home-favicon.png" />


            <Header cart={cart} />

            <div className="home-page">
                <ProductsGrid products={products} loadCart={loadCart} />
            </div>
        </>
    );
}