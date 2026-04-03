import { Link, NavLink, useNavigate} from 'react-router';
import './Header.css';
import logoWhite from '/src/assets/images/mobile-logo-white.png';
import mobileLogoWhite from "/src/assets/images/mobile-logo-white.png"
import searchIcon from "/src/assets/images/icons/search-icon.png"
import cartIcon from "/src/assets/images/icons/cart-icon.png"
import { useSearchParams } from 'react-router';
import { useState } from 'react';


export function Header({cart}) {

    const [searchParams]=useSearchParams();
    const search=searchParams.get("search");
    const [searchBarValue,setSearchBarValue]=useState(search ? search:"")
    const navigate=useNavigate();
    


    const handleSearchBarInput=(event)=>{

        setSearchBarValue(event.target.value)
    }

    const searchForItem=()=>{

        console.log(searchBarValue)
        navigate(`/?search=${searchBarValue}`)
    }

    let totalQuantity=0;
    cart.forEach((cartItem)=>{
        totalQuantity+=cartItem.quantity;
    })

    return (
        <>
            <div className="header">
                <div className="left-section">
                    <Link to="/" className="header-link">
                        <img className="logo"
                            src={logoWhite} />
                        <img className="mobile-logo"
                            src={mobileLogoWhite} />
                    </Link>
                </div>

                <div className="middle-section">
                    <input className="search-bar" type="text" placeholder="Search" value={searchBarValue} onChange={handleSearchBarInput} />

                    <button className="search-button" onClick={searchForItem}>
                        <img className="search-icon" src={searchIcon} />
                    </button>
                </div>

                <div className="right-section">
                    <NavLink className="orders-link header-link" to="/orders">

                        <span className="orders-text">Orders</span>
                    </NavLink>

                    <Link className="cart-link header-link" to="/checkout">
                        <img className="cart-icon" src={cartIcon} />
                        <div className="cart-quantity">{totalQuantity}</div>
                        <div className="cart-text">Cart</div>
                    </Link>
                </div>
            </div>
        </>
    )
}