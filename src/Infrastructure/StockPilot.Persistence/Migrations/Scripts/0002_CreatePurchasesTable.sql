create table purchases (
    id uuid not null,
    supplier_id uuid not null,
    product_id uuid not null,
    quantity integer not null,
    unit_price numeric(18, 2) not null,
    purchase_date timestamp with time zone not null,
    created_at timestamp with time zone not null,

    constraint pk_purchases primary key (id),
    constraint fk_purchases_supplier foreign key (supplier_id) references suppliers (id),
    constraint fk_purchases_product foreign key (product_id) references products (id),

    constraint ck_purchases_quantity_positive check (quantity > 0),
    constraint ck_purchases_unit_price_non_negative check (unit_price >= 0)
);

create index ix_purchases_supplier_id
    on purchases (supplier_id);

create index ix_purchases_product_id
    on purchases (product_id);

create index ix_purchases_purchase_date_created_at
    on purchases (purchase_date, created_at);
