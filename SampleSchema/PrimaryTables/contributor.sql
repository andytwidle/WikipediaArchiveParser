-- Table: public.contributor

-- DROP TABLE IF EXISTS public.contributor;

CREATE TABLE IF NOT EXISTS public.contributor
(
    id bigint NOT NULL,
    isgnome boolean DEFAULT false,
    CONSTRAINT contributor_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.contributor
    OWNER to postgres;