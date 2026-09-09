import { it, expect, describe, vi, beforeEach } from 'vitest'
import { render, screen, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event';
import { PaymentSummary } from './PaymentSummary';
import axios from 'axios';
import { MemoryRouter, useLocation } from 'react-router';

vi.mock('axios')
describe('PaymentSummary component', () => {
    let loadCart;
    let user;
    let paymentSummary;
    beforeEach(() => {
        loadCart = vi.fn();
        paymentSummary = {
            "totalItems": 5,
            "productCostCents": 7264,
            "shippingCostCents": 499,
            "totalCostBeforeTaxCents": 7763,
            "taxCents": 776,
            "totalCostCents": 8539
        }

        user = userEvent.setup();




    })

    it('shows correct payment amounts', async () => {

        function Location() {
            const location = useLocation();
            return <div data-testid="url-path">{location.pathname}</div>;
        }
        render(
            <MemoryRouter>
                <PaymentSummary
                    paymentSummary={paymentSummary}
                    loadCart={loadCart}
                />
                <Location />
            </MemoryRouter>
        )

        const itemsSummary = screen.getByTestId('items-summary')
        const shippingSummary = screen.getByTestId('shipping-summary')
        const subtotalSummary = screen.getByTestId('subtotal-summary')
        const taxSummary = screen.getByTestId('tax-summary')
        const totalSummary = screen.getByTestId('total-summary')


        expect(within(itemsSummary).getByText('Items (5):')).toBeInTheDocument();
        expect(within(shippingSummary).getByText('$4.99')).toBeInTheDocument();
        expect(within(subtotalSummary).getByText('$77.63')).toBeInTheDocument();
        expect(within(taxSummary).getByText('$7.76')).toBeInTheDocument();
        expect(within(totalSummary).getByText('$85.39')).toBeInTheDocument();



    })

    it('Places an order', async () => {

        function Location() {
            const location = useLocation();
            return <div data-testid="url-path">{location.pathname}</div>;
        }
        render(
            <MemoryRouter>
                <PaymentSummary
                    paymentSummary={paymentSummary}
                    loadCart={loadCart}
                />
                <Location />
            </MemoryRouter>
        )

        const placeOrderButton = screen.getByTestId('placeOrderButton')
        await user.click(placeOrderButton)
        expect(axios.post).toHaveBeenCalledWith('/api/orders')
        expect(loadCart).toHaveBeenCalled();
        expect(screen.getByTestId('url-path')).toHaveTextContent('/orders');

    })
})
