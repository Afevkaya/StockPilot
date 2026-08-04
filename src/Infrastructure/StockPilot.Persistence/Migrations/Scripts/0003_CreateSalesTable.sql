create table Sales
(
    id uuid not null,
    product_id uuid not null,
    quantity integer not null,
    unit_price numeric(18, 2) not null,
    sale_date timestamp with time zone not null,
    created_at timestamp with time zone not null,

    constraint pk_sales primary key (id),
    constraint fk_sales_product foreign key (product_id) references products (id),
    constraint ck_sales_quantity_positive check (quantity > 0),
    constraint ck_sales_unit_price_non_negative check (unit_price >= 0)
);

create index ix_sales_product_id
    on Sales (product_id);

create index ix_sales_sale_date_created_at
    on Sales (sale_date, created_at);
