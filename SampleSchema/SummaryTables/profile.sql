-- Table: public.profile

-- DROP TABLE IF EXISTS public.profile;

CREATE TABLE IF NOT EXISTS public.profile
(
    contributorid bigint NOT NULL,
    isgnome boolean NOT NULL DEFAULT false,
    r0 integer NOT NULL DEFAULT 0,
    r1 integer NOT NULL DEFAULT 0,
    r2 integer NOT NULL DEFAULT 0,
    r3 integer NOT NULL DEFAULT 0,
    r4 integer NOT NULL DEFAULT 0,
    r5 integer NOT NULL DEFAULT 0,
    r6 integer NOT NULL DEFAULT 0,
    r7 integer NOT NULL DEFAULT 0,
    r8 integer NOT NULL DEFAULT 0,
    r9 integer NOT NULL DEFAULT 0,
    r10 integer NOT NULL DEFAULT 0,
    r11 integer NOT NULL DEFAULT 0,
    r12 integer NOT NULL DEFAULT 0,
    r13 integer NOT NULL DEFAULT 0,
    r14 integer NOT NULL DEFAULT 0,
    r15 integer NOT NULL DEFAULT 0,
    r100 integer NOT NULL DEFAULT 0,
    r101 integer NOT NULL DEFAULT 0,
    r118 integer NOT NULL DEFAULT 0,
    r119 integer NOT NULL DEFAULT 0,
    r710 integer NOT NULL DEFAULT 0,
    r711 integer NOT NULL DEFAULT 0,
    r828 integer NOT NULL DEFAULT 0,
    r829 integer NOT NULL DEFAULT 0,
    minor integer NOT NULL DEFAULT 0,
    total integer NOT NULL DEFAULT 0,
    CONSTRAINT profiles_pkey PRIMARY KEY (contributorid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.profile
    OWNER to postgres;