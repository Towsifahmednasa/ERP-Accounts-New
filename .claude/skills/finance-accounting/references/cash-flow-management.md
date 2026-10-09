# Cash Flow Management

## Cash vs. Profit

**Critical distinction:** Profit ≠ Cash

A startup can be "profitable" on paper while running out of cash, or burning cash while showing accounting losses.

| Scenario | P&L Shows | Cash Reality |
|----------|-----------|--------------|
| Deferred revenue | No revenue yet | Cash in bank |
| AR outstanding | Revenue recognized | No cash yet |
| Prepaid annual contracts | Expense spread over year | Cash out immediately |
| Accrued expenses | Expense recognized | Cash not yet paid |

**Rule:** Manage cash flow separately from P&L. Both matter.

## 13-Week Cash Flow Forecast

The standard for startup cash management — weekly granularity, 3-month horizon.

### Structure

```
Week | Beginning Cash | Operating Receipts | Operating Disbursements | Net Operating | Financing | Ending Cash
-----|----------------|--------------------|-----------------------|---------------|-----------|------------
W1   | $500,000       | $45,000            | $75,000               | -$30,000      | $0        | $470,000
W2   | $470,000       | $50,000            | $72,000               | -$22,000      | $0        | $448,000
...
```

### Operating Receipts (Cash In)
- Customer payments (by expected timing)
- Refunds received
- Interest income
- Other operating income

### Operating Disbursements (Cash Out)
- Payroll (by pay period)
- Rent (by due date)
- Vendor payments (by terms)
- Software subscriptions (by billing date)
- Credit card payments
- Tax payments

### Financing Activities
- Equity investments
- Debt draws
- Debt payments
- Investor distributions

## Forecasting Best Practices

### Revenue/Receipts Forecasting

**By payment terms:**
| Customer Type | Typical Terms | Forecast Timing |
|---------------|---------------|-----------------|
| Self-serve (card) | Immediate | Week of sale |
| SMB invoiced | Net 30 | 4-5 weeks after sale |
| Mid-market | Net 30-45 | 5-7 weeks after sale |
| Enterprise | Net 45-60 | 7-9 weeks after sale |

**Adjustments:**
- Apply historical collection rate (e.g., 95% of AR collects)
- Factor in seasonal patterns
- Account for large one-time payments

### Expense Forecasting

**Fixed costs (predictable):**
- Payroll — by pay period, known amounts
- Rent — monthly, fixed date
- Software subscriptions — monthly/annual, known dates
- Insurance — quarterly/annual, known dates

**Variable costs (estimate):**
- AWS/hosting — based on growth projection
- Contractors — based on project plans
- Travel — based on scheduled trips
- Marketing spend — based on budget

**One-time costs (flag separately):**
- Equipment purchases
- Legal/fundraising costs
- Security deposits
- Conference sponsorships

## Accounts Receivable Management

### AR Aging Report

| Aging Bucket | Amount | % of Total | Action |
|--------------|--------|------------|--------|
| Current (0-30 days) | $50,000 | 50% | Monitor |
| 31-60 days | $30,000 | 30% | Follow up |
| 61-90 days | $15,000 | 15% | Escalate |
| 90+ days | $5,000 | 5% | Collection/write-off |

### Collection Best Practices

**Before invoice:**
- Verify customer contact info
- Confirm PO requirements
- Set up auto-pay when possible

**At invoice:**
- Send immediately upon delivery
- Include clear payment instructions
- Offer multiple payment methods

**Collection cadence:**
| Day | Action |
|-----|--------|
| Due date | Payment reminder (automated) |
| +7 days | Friendly follow-up email |
| +14 days | Phone call + escalate to account owner |
| +30 days | Formal collection notice |
| +45 days | Consider collection agency or write-off |

**Acceleration tactics:**
- Offer 2/10 net 30 (2% discount for payment in 10 days)
- Accept credit card for immediate payment
- Offer payment plans for large invoices
- Invoice deposits/milestones vs. full amount at end

## Accounts Payable Management

### AP Optimization

**Extend terms where possible:**
| Vendor Type | Standard | Target |
|-------------|----------|--------|
| Software | Monthly | Annual (for discount) |
| Professional services | Due on receipt | Net 30 |
| Contractors | Net 15 | Net 30 |
| Major vendors | Net 30 | Net 45-60 |

**Payment timing strategy:**
- Pay on due date, not before
- Batch payments weekly vs. daily
- Use credit cards for cash flow buffer (30+ days float)
- Negotiate extended terms with key vendors

**Early payment discounts:**
Only take if: Discount % > (365 / (Full term - Discount term)) × Your cost of capital

Example: 2/10 net 30 = 36% annualized return. Take it if your cost of capital <36%.

## Cash Buffer Guidelines

### Minimum Cash Reserve

| Stage | Minimum Reserve | Rationale |
|-------|-----------------|-----------|
| Pre-seed | 6 months runway | Long fundraise cycles |
| Seed | 3-4 months expenses | Buffer for unexpected |
| Series A+ | 2-3 months expenses | More predictable |

### Cash Positioning

```
Operating Account:  2-3 months of expenses (daily operations)
Reserve Account:    Remaining cash (earn interest)
Payroll Account:    Next payroll + buffer (if separate)
```

**Money market/savings options:**
- Mercury Treasury
- Brex high-yield
- Wealthfront Cash
- Traditional bank sweep accounts

Target: Earn yield on idle cash without sacrificing liquidity.

## Scenario Planning

### Three Scenarios

**Base Case:** Expected performance
- Revenue hits forecast
- Expenses as planned
- No major surprises

**Upside Case:** Things go well (+20-30%)
- Revenue beats by 20%+
- Key deals close early
- Expenses on track

**Downside Case:** Things go poorly (-20-30%)
- Revenue misses by 20%+
- Key deals slip
- Unexpected expenses

### Stress Testing

Answer these questions:
1. At current burn, how long until cash runs out?
2. If revenue drops 50%, how long?
3. What expenses could we cut to extend runway 6 months?
4. When is the latest we can start fundraising?

## Cash Flow Red Flags

| Signal | Warning Level | Action |
|--------|---------------|--------|
| AR >60 days growing | Yellow | Tighten collections |
| Cash < 6 months runway | Yellow | Start contingency planning |
| Cash < 3 months runway | Red | Cut costs, start fundraise |
| Cash declining faster than forecast | Red | Investigate immediately |
| Large customer concentration | Yellow | Diversify, monitor closely |

## Weekly Cash Review Checklist

- [ ] Review current cash balance vs. forecast
- [ ] Check AR aging, follow up on overdue
- [ ] Review upcoming AP obligations
- [ ] Update 13-week forecast with actuals
- [ ] Identify any large upcoming cash needs
- [ ] Calculate current runway
