-- Table: public.contributor_words

-- DROP TABLE IF EXISTS public.contributor_words;

CREATE TABLE IF NOT EXISTS public.contributor_words
(
    contributorid integer NOT NULL,
    words integer[],
    CONSTRAINT contributor_words_pkey PRIMARY KEY (contributorid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.contributor_words
    OWNER to postgres;