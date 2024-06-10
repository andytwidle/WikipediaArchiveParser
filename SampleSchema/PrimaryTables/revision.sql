-- Table: public.revision

-- DROP TABLE IF EXISTS public.revision;

CREATE TABLE IF NOT EXISTS public.revision
(
    id bigint NOT NULL,
    pageid bigint,
    parentrevisionid bigint,
    timestamputc timestamp without time zone,
    contributorid bigint,
    minorrevision boolean NOT NULL,
    textbytes bigint,
    revisioncomment character varying COLLATE pg_catalog."default",
    CONSTRAINT revision_pkey PRIMARY KEY (id),
    CONSTRAINT fk_pageid FOREIGN KEY (pageid)
        REFERENCES public.page (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.revision
    OWNER to postgres;
-- Index: revision_contributorid_idx

-- DROP INDEX IF EXISTS public.revision_contributorid_idx;

CREATE INDEX IF NOT EXISTS revision_contributorid_idx
    ON public.revision USING btree
    (contributorid ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;
-- Index: revision_contributorid_pageid_idx

-- DROP INDEX IF EXISTS public.revision_contributorid_pageid_idx;

CREATE INDEX IF NOT EXISTS revision_contributorid_pageid_idx
    ON public.revision USING btree
    (contributorid ASC NULLS LAST, pageid ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;
-- Index: revision_pageid_idx

-- DROP INDEX IF EXISTS public.revision_pageid_idx;

CREATE INDEX IF NOT EXISTS revision_pageid_idx
    ON public.revision USING btree
    (pageid ASC NULLS LAST)
    WITH (deduplicate_items=True)
    TABLESPACE pg_default;