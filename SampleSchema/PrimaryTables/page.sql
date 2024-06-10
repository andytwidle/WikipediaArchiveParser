-- Table: public.page

-- DROP TABLE IF EXISTS public.page;

CREATE TABLE IF NOT EXISTS public.page
(
    id bigint NOT NULL,
    namespaceid integer,
    title character varying COLLATE pg_catalog."default",
    CONSTRAINT page_pkey PRIMARY KEY (id),
    CONSTRAINT title_not_null CHECK (title IS NOT NULL)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.page
    OWNER to postgres;
-- Index: page_id_namespaceid_idx

-- DROP INDEX IF EXISTS public.page_id_namespaceid_idx;

CREATE INDEX IF NOT EXISTS page_id_namespaceid_idx
    ON public.page USING btree
    (id ASC NULLS LAST, namespaceid ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;
-- Index: page_namespaceid_id_idx

-- DROP INDEX IF EXISTS public.page_namespaceid_id_idx;

CREATE INDEX IF NOT EXISTS page_namespaceid_id_idx
    ON public.page USING btree
    (namespaceid ASC NULLS LAST)
    INCLUDE(id)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;