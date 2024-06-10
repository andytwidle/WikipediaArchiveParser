-- Table: public.words

-- DROP TABLE IF EXISTS public.words;

CREATE TABLE IF NOT EXISTS public.words
(
    id integer NOT NULL DEFAULT nextval('words_id_seq'::regclass),
    word character varying COLLATE pg_catalog."default",
    CONSTRAINT words_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.words
    OWNER to postgres;