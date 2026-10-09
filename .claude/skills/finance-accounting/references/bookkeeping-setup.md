# Bookkeeping Setup Guide

## Software Comparison

### QuickBooks Online

**Best for:** US-based startups, those needing integrated payroll, accountant familiarity

**Pricing (2025):**
- Simple Start: $30/month (1 user)
- Essentials: $60/month (3 users)
- Plus: $90/month (5 users)
- Advanced: $200/month (25 users)

**Strengths:**
- 80% US market share — most accountants know it
- Robust reporting and customization
- Built-in payroll option
- Strong expense tracking with receipt capture
- Industry-specific chart of accounts templates

**Weaknesses:**
- Per-user pricing adds up
- Can be overwhelming for simple needs
- Some features require higher tiers

### Xero

**Best for:** Startups, international companies, teams needing unlimited users

**Pricing (2025):**
- Early: $15/month (limited invoices/bills)
- Growing: $42/month (unlimited)
- Established: $78/month (multi-currency, projects)

**Strengths:**
- Unlimited users on ALL plans
- Clean, modern interface
- Excellent multi-currency support
- 1000+ app integrations
- True double-entry bookkeeping on all plans

**Weaknesses:**
- Fewer US accountants are certified
- Some advanced reports require add-ons
- Receipt capture on higher tiers only

### Wave

**Best for:** Pre-revenue startups, bootstrapped founders

**Pricing:** Free (revenue from payment processing)

**Strengths:**
- Completely free accounting
- Simple and intuitive
- Unlimited users

**Weaknesses:**
- Limited integrations
- No inventory tracking
- Support can be slow

### Bench

**Best for:** Founders who want hands-off bookkeeping

**Pricing:** $299-$499/month (done-for-you)

**Strengths:**
- Human bookkeepers do the work
- Monthly financial statements delivered
- Tax-ready books

**Weaknesses:**
- Higher cost
- Less control over categorization
- Not real-time

## Selection Decision Tree

```
Start Here
    │
    ├── International or multi-currency? 
    │   └── YES → Xero (best multi-currency)
    │
    ├── Pre-revenue or bootstrapped?
    │   └── YES → Wave (free) or Xero Early ($15)
    │
    ├── Need integrated payroll?
    │   └── YES → QuickBooks (native) or Xero + Gusto
    │
    ├── >3 team members need access?
    │   └── YES → Xero (unlimited users)
    │
    ├── US-based with US accountant?
    │   └── YES → QuickBooks (accountant preference)
    │
    └── Want someone else to do it?
        └── YES → Bench or Pilot
```

## Chart of Accounts Setup

### Startup-Optimized Chart of Accounts

**Assets (1000s)**
```
1000 - Cash and Cash Equivalents
  1010 - Operating Checking
  1020 - Savings/Money Market
  1030 - Payroll Account
1100 - Accounts Receivable
1200 - Prepaid Expenses
  1210 - Prepaid Insurance
  1220 - Prepaid Software
1300 - Other Current Assets
1500 - Fixed Assets
  1510 - Computer Equipment
  1520 - Furniture & Fixtures
1600 - Accumulated Depreciation
```

**Liabilities (2000s)**
```
2000 - Accounts Payable
2100 - Credit Cards
2200 - Accrued Expenses
  2210 - Accrued Payroll
  2220 - Accrued Benefits
2300 - Deferred Revenue
2400 - Notes Payable (short-term)
2500 - Long-term Debt
```

**Equity (3000s)**
```
3000 - Common Stock
3100 - Additional Paid-in Capital
3200 - Retained Earnings
3300 - Owner's Draw/Distributions
```

**Revenue (4000s)**
```
4000 - Product Revenue
  4010 - Subscription Revenue
  4020 - One-time Revenue
4100 - Service Revenue
  4110 - Professional Services
  4120 - Support Revenue
4200 - Other Revenue
```

**Cost of Goods Sold (5000s)**
```
5000 - Cost of Revenue
  5010 - Hosting & Infrastructure
  5020 - Payment Processing Fees
  5030 - Third-party Software (in product)
  5040 - Customer Support (direct)
```

**Operating Expenses (6000s-8000s)**
```
6000 - Sales & Marketing
  6010 - Salaries - Sales
  6020 - Salaries - Marketing
  6030 - Advertising & Paid Media
  6040 - Events & Conferences
  6050 - Sales Tools (CRM, etc.)

7000 - Research & Development
  7010 - Salaries - Engineering
  7020 - Salaries - Product
  7030 - Salaries - Design
  7040 - Development Tools
  7050 - Contractors - Development

8000 - General & Administrative
  8010 - Salaries - Executive
  8020 - Salaries - Operations
  8030 - Legal & Professional Fees
  8040 - Accounting & Tax
  8050 - Insurance
  8060 - Rent & Facilities
  8070 - Software Subscriptions (G&A)
  8080 - Travel & Meals
  8090 - Bank Fees & Interest
```

## Daily/Weekly Bookkeeping Practices

### Daily (5 minutes)
- Review bank transactions for fraud/errors
- Categorize 2-3 transactions if time permits

### Weekly (30 minutes)
- Reconcile all accounts
- Categorize all transactions
- Review outstanding invoices
- Follow up on late payments

### Monthly (2-4 hours)
- Full reconciliation of all accounts
- Review and adjust accruals
- Generate financial statements
- Review budget vs. actual
- Archive receipts and documentation

## Common Categorization Rules

| Transaction Type | Category | Notes |
|------------------|----------|-------|
| AWS/GCP/Azure | 5010 - Hosting | COGS |
| Stripe/PayPal fees | 5020 - Payment Processing | COGS |
| Slack/Notion/Asana | 8070 - Software (G&A) | OpEx |
| Figma/GitHub | 7040 - Dev Tools | R&D |
| Salesforce/HubSpot | 6050 - Sales Tools | S&M |
| Google/Facebook ads | 6030 - Advertising | S&M |
| Contractor (dev) | 7050 - Contractors | R&D |
| Contractor (design) | 7050 - Contractors | R&D |
| Lawyer fees | 8030 - Legal | G&A |
| Accountant fees | 8040 - Accounting | G&A |

## Integration Recommendations

### Essential Integrations
1. **Bank feeds** — Auto-import transactions
2. **Payroll** — Gusto, Rippling, or native
3. **Expense management** — Ramp, Brex, or Expensify
4. **Payment processing** — Stripe, Square

### Nice-to-Have Integrations
1. **CRM** — Revenue recognition sync
2. **Bill pay** — Bill.com, Melio
3. **Inventory** — If applicable
4. **Time tracking** — For services revenue

## Red Flags to Avoid

- **Mixing accounts:** Keep business and personal separate from day one
- **Cash basis too long:** Switch to accrual before raising
- **Inconsistent categories:** Document your rules
- **Delayed reconciliation:** Don't let it pile up
- **Missing receipts:** Capture everything >$75
