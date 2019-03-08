create table if not exists `student_rating_base`.users
(
  id       bigint auto_increment primary key,
  login    varchar(256) null,
  password int          null,
  role     varchar(256) null,
  constraint users_id_uindex
  unique (id),
  constraint users_role_uindex
  unique (role)
);

create table if not exists `student_rating_base`.students
(
  id           bigint auto_increment primary key,
  group_number int          null,
  name         varchar(256) null,
  surname      varchar(256) null,
  patronymic   varchar(256) null,
  constraint students_id_uindex
  unique (id)
);

create table if not exists `student_rating_base`.rating
(
  id                  bigint auto_increment primary key,
  student_id          bigint       null,
  history             varchar(256) null,
  `political _cience` varchar(256) null,
  PE                  varchar(256) null,
  foreign_language    varchar(256) null,
  chemistry           varchar(256) null,
  philosophy          int          null,
  psychology          int          null,
  mathematics         int          null,
  physics             int          null,
  programming         int          null,
  constraint rating_id_uindex
  unique (id),
  constraint rating_student_id_uindex
  unique (student_id),
  constraint rating_students_id_fk
  foreign key (student_id) references students (id)
    on update cascade
    on delete cascade
);

insert ignore into student_rating_base.users
values (1, 'dan', 1, 'admin'),
       (2, 'alf', 1, 'user'); 

insert ignore into student_rating_base.students
values (1, 000001, 'Александр', 'Устапенок', 'Владимирович'),
       (2, 000001, 'Курилл', 'Шум', 'Алексеевич'),
       (3, 000001, 'Ромун', 'Виилов', 'Богданович'),
       (4, 000001, 'Улександр', 'Червяков', 'Степанович'),
       (5, 000001, 'Данила', 'Самусев', 'Андреевич'),
       (6, 000002, 'Даниил', 'Драгн', 'Петрович'),
       (7, 000002, 'Хагер', 'Альфред', 'Хайнц Харальд'),
       (8, 000002, 'Карапетян', 'Артём', 'Артурович'),
       (9, 000002, 'Тачпад', 'Алексей', 'Денисович'),
       (10, 000002, 'Денис', 'Лажуков', 'Анатольевич');

insert ignore into student_rating_base.rating 
values(1, 1, 'зачёт', 'зачёт', 'незачёт', 'зачёт', 'незачёт', 9, 10, 8, 7, 8),
      (2, 2, 'незачёт', 'незачёт', 'зачёт', 'незачёт', 'незачёт', 8, 6, 6, 5, 7),
      (3, 3, 'зачёт', 'зачёт', 'незачёт', 'незачёт', 'зачёт', 8, 7, 6, 10, 9),
      (4, 4, 'незачёт', 'зачёт', 'незачёт', 'зачёт', 'зачёт', 9, 6, 9, 6, 1),
      (5, 5, 'зачёт', 'незачёт', 'зачёт', 'зачёт', 'незачёт', 4, 7, 9, 8, 7),
      (6, 6, 'незачёт', 'зачёт', 'незачёт', 'зачёт', 'незачёт', 8, 5, 7, 9, 6),
      (7, 7, 'зачёт', 'незачёт', 'зачёт', 'незачёт', 'зачёт', 7, 9, 10, 6, 6),
      (8, 8, 'незачёт', 'незачёт', 'зачёт', 'зачёт', 'незачёт', 8, 7, 5, 6, 7),
      (9, 9, 'незачёт', 'зачёт', 'незачёт', 'зачёт', 'незачёт', 6, 10, 9, 8, 7),
      (10, 10, 'зачёт', 'зачёт', 'зачёт', 'незачёт', 'незачёт', 6, 7, 7, 10, 8);
