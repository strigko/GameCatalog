DELETE FROM Games;
DBCC CHECKIDENT ('Games', RESEED, 0);


INSERT INTO Games (Name, Description, Price, YearOfRelease, PictureFilePath) VALUES
('The Witcher 3',         'Open world RPG.',                             399.99, 2015, 'witcher3.jpg'),
('Cyberpunk 2077',        'Futuristic RPG.',                             599.99, 2020, 'cyberpunk.jpg'),
('Red Dead Redemption 2', 'Western open-world adventure.',               499.99, 2018, 'rdr2.jpg'),
('Elden Ring',            'Challenging souls-like RPG.',                 699.99, 2022, 'eldenring.jpg'),
('God of War',            'Kratos mythological adventure.',              549.99, 2018, 'gow.jpg'),
('GTA V',                 'Open-world crime simulator.',                 849.99, 2013, 'gta5.jpg'),
('Hogwarts Legacy',       'Wizarding world RPG.',                        479.99, 2023, 'hogwarts.jpg'),
('God of War Ragnarok',   'Continuation of Kratos story.',               599.99, 2022, 'gowr.jpg'),
('The Last of Us',        'Post-apocalyptic survival story.',            449.99, 2013, 'tlou.jpg'),

('The Last of Us Part II','Emotional sequel adventure.',                 599.99, 2020, 'tlou2.jpg'),
('Assassin''s Creed Valhalla','Viking open-world RPG.',                  599.99, 2020, 'acvalhalla.jpg'),
('Assassin''s Creed Odyssey','Ancient Greece RPG.',                      499.99, 2018, 'acodyssey.jpg'),
('Assassin''s Creed Origins','Egyptian open-world RPG.',                 449.99, 2017, 'acorigins.jpg'),
('Far Cry 6',             'Open-world shooter in Yara.',                 499.99, 2021, 'farcry6.jpg'),
('Far Cry 5',             'Cult-themed FPS.',                            399.99, 2018, 'farcry5.jpg'),

('Call of Duty: Modern Warfare','Realistic military FPS.',               599.99, 2019, 'codmw.jpg'),
('Call of Duty: Warzone','Battle royale shooter.',                       0.00, 2020, 'warzone.jpg'),
('Battlefield 2042',      'Large-scale futuristic battles.',             599.99, 2021, 'bf2042.jpg'),
('Battlefield V',         'WWII multiplayer shooter.',                   499.99, 2018, 'bfv.jpg'),

('Dark Souls III',        'Hardcore action RPG.',                        399.99, 2016, 'ds3.jpg'),
('Sekiro: Shadows Die Twice','Samurai action game.',                     549.99, 2019, 'sekiro.jpg'),
('Bloodborne',            'Gothic horror RPG.',                          399.99, 2015, 'bloodborne.jpg'),

('Horizon Zero Dawn',     'Post-apocalyptic robot world.',               399.99, 2017, 'hzd.jpg'),
('Horizon Forbidden West','Sequel open-world RPG.',                      599.99, 2022, 'hfw.jpg'),

('Spider-Man',            'Superhero open-world action.',                499.99, 2018, 'spiderman.jpg'),
('Spider-Man Miles Morales','Spin-off superhero story.',                 549.99, 2020, 'spidermanmm.jpg'),

('Uncharted 4',           'Treasure hunting adventure.',                 399.99, 2016, 'uncharted4.jpg'),
('Uncharted Lost Legacy', 'Standalone Uncharted story.',                 349.99, 2017, 'unchartedll.jpg'),

('Death Stranding',       'Unique delivery adventure.',                  499.99, 2019, 'deathstranding.jpg'),
('Resident Evil Village', 'Survival horror sequel.',                     549.99, 2021, 'revillage.jpg'),
('Resident Evil 4 Remake','Modern remake of classic horror.',            599.99, 2023, 're4.jpg'),

('DOOM Eternal',          'Fast-paced demon shooter.',                   499.99, 2020, 'doometernal.jpg'),
('DOOM (2016)',           'Reboot of classic shooter.',                  399.99, 2016, 'doom.jpg'),

('Final Fantasy XV',      'Fantasy RPG adventure.',                      499.99, 2016, 'ff15.jpg'),
('Final Fantasy VII Remake','Modern remake RPG.',                        599.99, 2020, 'ff7r.jpg'),

('Monster Hunter World',  'Co-op monster hunting.',                      449.99, 2018, 'mhw.jpg'),
('Dragon Age Inquisition','Fantasy RPG.',                                399.99, 2014, 'dai.jpg'),

('Mass Effect Legendary Edition','Sci-fi RPG trilogy remaster.',         599.99, 2021, 'melegendary.jpg'),
('The Division 2',        'Online looter shooter.',                      399.99, 2019, 'division2.jpg'),

('Watch Dogs Legion',     'Hacking open-world game.',                    499.99, 2020, 'wdlegion.jpg'),
('Metro Exodus',          'Post-apocalyptic FPS.',                       449.99, 2019, 'metroexodus.jpg'),
('Control',               'Supernatural action game.',                   399.99, 2019, 'control.jpg'),
('Alan Wake 2',           'Psychological horror.',                       599.99, 2023, 'alanwake2.jpg');