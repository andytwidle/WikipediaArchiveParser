-- Table: public.revision_words

-- DROP TABLE IF EXISTS public.revision_words;

CREATE TABLE IF NOT EXISTS public.revision_words
(
    revisionid integer NOT NULL,
    words bit(100),
    CONSTRAINT revision_words_pkey PRIMARY KEY (revisionid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.revision_words
    OWNER to postgres;