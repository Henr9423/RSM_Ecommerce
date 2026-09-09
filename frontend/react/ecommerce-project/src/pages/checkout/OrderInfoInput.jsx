import "./OrderInfoInput.css"
export function OrderInfoInput({orderInfo, setOrderInfo}) {


  function handleChange(event) {
    const { name, value } = event.target;

    setOrderInfo(prev => ({
      ...prev,
      [name]: value
    }));
  }
    return(
        <>
        <div className="order-info-input">

            <input 
                type="text" 
                name="firstName" 
                value={orderInfo.firstName} 
                onChange={handleChange} 
                autoComplete="given-name"
                placeholder="First name">
            </input>
            
            <input 
                type="text" 
                name="lastName" 
                value={orderInfo.lastName} 
                onChange={handleChange} 
                autoComplete="family-name"
                placeholder="Last name">
            </input>

            <input 
                type="email" 
                name="email" 
                value={orderInfo.email} 
                onChange={handleChange} 
                autoComplete="email"
                placeholder="Email">
            </input>

            <input 
                type="text" 
                name="addressLine1" 
                value={orderInfo.addressLine1} 
                onChange={handleChange} 
                autoComplete="shipping address-line1"
                placeholder="Address">
            </input>

            <input 
                type="text" 
                name="addressLine2" 
                value={orderInfo.addressLine2} 
                onChange={handleChange} 
                autoComplete="shipping address-line2"
                placeholder="Appartment, suite, etc">
            </input>

            <input 
                type="text" 
                name="city" 
                value={orderInfo.city} 
                onChange={handleChange} 
                autoComplete="shipping address-level2"
                placeholder="City">
            </input>

            <input 
                type="text" 
                name="postalCode" 
                value={orderInfo.postalCode} 
                onChange={handleChange} 
                autoComplete="shipping postal-code"
                placeholder="Postal code">
            </input>

            <input 
                type="text" 
                name="country" 
                value={orderInfo.country} 
                onChange={handleChange} 
                autoComplete="shipping country-name"
                placeholder="Country">
            </input>

            <input 
                type="tel" 
                name="phoneNumber" 
                value={orderInfo.phoneNumber} 
                onChange={handleChange} 
                autoComplete="tel"
                placeholder="Phone number">
            </input>

        


            <label>
                <input
                    type="checkbox"
                    checked={orderInfo.billingSameAsShipping}
                    onChange={(e) =>
                    setOrderInfo(prev => ({
                        ...prev,
                        billingSameAsShipping: e.target.checked
                    }))
                    }
                />

                Billing address is the same as shipping address
            </label>

            {!orderInfo.billingSameAsShipping && (
                <>
                    <input
                    type="text"
                    name="billingAddressLine1"
                    placeholder="Billing address"
                    autoComplete="billing address-line1"
                    value={orderInfo.billingAddressLine1}
                    onChange={handleChange}
                    />

                    <input
                    type="text"
                    name="billingAddressLine2"
                    placeholder="Appartment, suite, etc"
                    autoComplete="billing address-line2"
                    value={orderInfo.billingAddressLine2}
                    onChange={handleChange}
                    />

                    <input
                    type="text"
                    name="billingCity"
                    placeholder="City"
                    autoComplete="billing address-level2"
                    value={orderInfo.billingCity}
                    onChange={handleChange}
                    />

                    <input
                    type="text"
                    name="billingStateOrRegion"
                    placeholder="State / Region"
                    autoComplete="billing address-level1"
                    value={orderInfo.billingStateOrRegion}
                    onChange={handleChange}
                    />

                    <input
                    type="text"
                    name="billingPostalCode"
                    placeholder="Postal code"
                    autoComplete="billing postal-code"
                    value={orderInfo.billingPostalCode}
                    onChange={handleChange}
                    />

                    <input
                    type="text"
                    name="billingCountry"
                    placeholder="Country"
                    autoComplete="billing country-name"
                    value={orderInfo.billingCountry}
                    onChange={handleChange}
                    />
                </>
            )}
                
        </div>
    </>)
}