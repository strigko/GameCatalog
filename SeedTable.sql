DELETE FROM Games;
DBCC CHECKIDENT ('Games', RESEED, 0);

INSERT INTO Games (Name, Description, Price, YearOfRelease, PictureFilePath) VALUES
('The Witcher 3',         'Open world game.',                           399.99, 2015, 'witcher3.jpg'),
('Cyberpunk 2077',        'Futuristic RPG.',                            599.99, 2020, 'cyberpunk.jpg'),
('Red Dead Redemption 2', 'West Coast traveller.',                      499.99, 2018, 'rdr2.jpg'),
('Elden Ring',            'Really hard souls-like game.',               699.99, 2022, 'eldenring.jpg'),
('God of War',            'Adventures of Cratos and Antrey.',           549.99, 2018, 'gow.jpg'),
('GTA5',                  'Adventures of Franklin, Michael and Trevor.', 849.99, 2009, 'gta5.jpg'),
('Hogwarts Legacy',       'Magical world of Harry Potter.'             , 849.99, 2009, 'hogwarts.jpg'),
('God of War Ragnarok',   'Adventures of Cratos and Antrey pt2.'       , 849.99, 2009, 'gowr.jpg');