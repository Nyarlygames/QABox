<?xml version="1.0" encoding="UTF-8"?>
<tileset name="QABoxObjects" tilewidth="500" tileheight="600" tilecount="6" columns="0">
 <grid orientation="orthogonal" width="1" height="1"/>
 <tile id="0">
  <properties>
   <property name="spawner" type="bool" value="true"/>
  </properties>
  <image width="500" height="500" source="Spawner.png"/>
 </tile>
 <tile id="1">
  <image width="400" height="600" source="Panel_DontPlay.png"/>
 </tile>
 <tile id="2">
  <image width="400" height="600" source="Panel_Options.png"/>
 </tile>
 <tile id="3">
  <image width="400" height="600" source="Panel_Play.png"/>
 </tile>
 <tile id="4">
  <properties>
   <property name="camcollider" type="bool" value="true"/>
   <property name="visible" type="bool" value="false"/>
  </properties>
  <image width="500" height="500" source="CamLock.png"/>
 </tile>
 <tile id="5">
  <properties>
   <property name="collider" type="bool" value="true"/>
  </properties>
  <image width="500" height="500" source="Ladder.png"/>
 </tile>
</tileset>
