export function formatMoney(amountCents) {
   const isNegative=amountCents<0;
   const absolute=Math.abs(amountCents);
  
   return `${isNegative? '-': ''}$${(absolute/100).toFixed(2)}`
}