create table categories (
    id uuid not null,
    name varchar(100) not null,
    description text not null,
    created_at timestamp with time zone not null ,
    updated_at timestamp with time zone,

    constraint pk_categories primary key (id)
);

create table suppliers (
    id uuid not null,
    name varchar(150) not null,
    contact_name varchar(100),
    email varchar(255),
    phone varchar(30),
    address text,
    created_at timestamp with time zone not null ,
    updated_at timestamp with time zone,

    constraint pk_suppliers primary key (id)
);

create table products (
    id uuid not null,
    name varchar(150) not null,
    description text,
    purchase_price numeric(18, 2) not null,
    sale_price numeric(18, 2) not null,
    category_id uuid not null,
    created_at timestamp with time zone not null,
    updated_at timestamp with time zone,

    constraint pk_products primary key (id),
    constraint fk_products_category foreign key (category_id) references categories (id),
    constraint ck_products_purchase_price_non_negative
        check (purchase_price >= 0),

    constraint ck_products_sale_price_non_negative
        check (sale_price >= 0)
);

create index ix_products_category_id
    on products (category_id);

create table product_suppliers (
    product_id uuid not null,
    supplier_id uuid not null,

    constraint pk_product_suppliers
        primary key (product_id, supplier_id),
    constraint fk_product_suppliers_product
        foreign key (product_id)
        references products (id)
        on delete cascade,
    constraint fk_product_suppliers_supplier
        foreign key (supplier_id)
        references suppliers (id)
        on delete cascade
);

create index ix_product_suppliers_supplier_id
    on product_suppliers (supplier_id);

create table stock_movements (
    id uuid not null,
    product_id uuid not null,
    quantity integer not null,
    movement_type smallint not null, -- 0 for entry, 1 for exit
    created_at timestamp with time zone not null,

    constraint pk_stock_movements primary key (id),
    constraint fk_stock_movements_product foreign key (product_id) references products (id),
    constraint ck_stock_movements_quantity_positive check (quantity > 0),
    constraint ck_stock_movements_movement_type_valid check (movement_type in (0, 1))
);

create index ix_stock_movements_product_id
    on stock_movements (product_id);
